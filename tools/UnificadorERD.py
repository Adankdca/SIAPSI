import mysql.connector

def unificar_erd():
    print("Iniciando Unificación del Diagrama Entidad-Relación (ERD)...")
    
    try:
        conn = mysql.connector.connect(host='localhost', database='SIAP_DB', user='root', password='root')
        cursor = conn.cursor()
        
        # Función auxiliar para ignorar errores de llaves ya existentes
        def execute_sql(sql_query, desc):
            try:
                cursor.execute(sql_query)
                print(f" [OK] {desc}")
            except mysql.connector.Error as err:
                if err.errno == 1060 or err.errno == 1061 or err.errno == 1826 or err.errno == 1050: 
                    print(f" [IGNORADO] {desc} (Ya existe)")
                else:
                    print(f" [ERROR] {desc} -> {err}")

        print("\n1. Sellando Relaciones Jerárquicas Olvidadas...")
        execute_sql("ALTER TABLE Cat_UR ADD CONSTRAINT fk_ur_orgpub FOREIGN KEY (OrgPubId) REFERENCES Cat_OrgPub(Id)", "Cat_UR -> Cat_OrgPub")
        execute_sql("ALTER TABLE Cat_UnidadResponsableOperativa ADD CONSTRAINT fk_uro_ur FOREIGN KEY (UrId) REFERENCES Cat_UR(Id)", "Cat_UnidadResponsableOperativa -> Cat_UR")
        execute_sql("ALTER TABLE Cat_Municipio ADD CONSTRAINT fk_mun_reg FOREIGN KEY (RegionId) REFERENCES Cat_Region(Id)", "Cat_Municipio -> Cat_Region")
        execute_sql("ALTER TABLE Cat_Funcion ADD CONSTRAINT fk_fun_fin FOREIGN KEY (FinalidadId) REFERENCES Cat_Finalidad(Id)", "Cat_Funcion -> Cat_Finalidad")
        execute_sql("ALTER TABLE Cat_Subfuncion ADD CONSTRAINT fk_sfun_fun FOREIGN KEY (FuncionId) REFERENCES Cat_Funcion(Id)", "Cat_Subfuncion -> Cat_Funcion")
        execute_sql("ALTER TABLE Cat_Subsubfuncion ADD CONSTRAINT fk_ssfun_sfun FOREIGN KEY (SubfuncionId) REFERENCES Cat_Subfuncion(Id)", "Cat_Subsubfuncion -> Cat_Subfuncion")
        execute_sql("ALTER TABLE Cat_ProgramaFondo ADD CONSTRAINT fk_pf_tr FOREIGN KEY (TipoRecursoId) REFERENCES Cat_TipoRecurso(Id)", "Cat_ProgramaFondo -> Cat_TipoRecurso")
        execute_sql("ALTER TABLE Cat_ProgramaFondo ADD CONSTRAINT fk_pf_ff FOREIGN KEY (FuenteFinanciamientoId) REFERENCES Cat_FuenteFinanciamiento(Id)", "Cat_ProgramaFondo -> Cat_FuenteFinanciamiento")
        execute_sql("ALTER TABLE Cat_ProgramaFondo ADD CONSTRAINT fk_pf_ramo FOREIGN KEY (RamoId) REFERENCES Cat_Ramo(Id)", "Cat_ProgramaFondo -> Cat_Ramo")

        print("\n2. Conectando Diccionarios Deductivos (Capítulo, Tipo Recurso FF, Objeto Integrar)...")
        execute_sql("ALTER TABLE Cat_ObjetoGasto ADD COLUMN CapituloGastoId INT", "Añadir CapituloGastoId a Cat_ObjetoGasto")
        execute_sql("""
            UPDATE Cat_ObjetoGasto og 
            JOIN Cat_CapituloGasto cg ON LEFT(og.Clave, 1) = LEFT(cg.Clave, 1)
            SET og.CapituloGastoId = cg.Id
        """, "Mapear Capítulos de Gasto")
        execute_sql("ALTER TABLE Cat_ObjetoGasto ADD CONSTRAINT fk_og_cg FOREIGN KEY (CapituloGastoId) REFERENCES Cat_CapituloGasto(Id)", "Cat_ObjetoGasto -> Cat_CapituloGasto")

        execute_sql("ALTER TABLE Cat_ObjetoGastoIntegrar ADD COLUMN ObjetoGastoId INT", "Añadir ObjetoGastoId a ObjetoGastoIntegrar")
        execute_sql("ALTER TABLE Cat_ObjetoGastoIntegrar ADD CONSTRAINT fk_ogi_og FOREIGN KEY (ObjetoGastoId) REFERENCES Cat_ObjetoGasto(Id)", "Cat_ObjetoGastoIntegrar -> Cat_ObjetoGasto")
        
        execute_sql("ALTER TABLE Cat_TipoRecursoFuenteFinanciamiento ADD COLUMN TipoRecursoId INT, ADD COLUMN FuenteFinanciamientoId INT", "Añadir llaves a Cat_TipoRecursoFuenteFinanciamiento")
        execute_sql("ALTER TABLE Cat_TipoRecursoFuenteFinanciamiento ADD CONSTRAINT fk_trff_tr FOREIGN KEY (TipoRecursoId) REFERENCES Cat_TipoRecurso(Id)", "Cat_TipoRecursoFuenteFinanciamiento -> Cat_TipoRecurso")
        execute_sql("ALTER TABLE Cat_TipoRecursoFuenteFinanciamiento ADD CONSTRAINT fk_trff_ff FOREIGN KEY (FuenteFinanciamientoId) REFERENCES Cat_FuenteFinanciamiento(Id)", "Cat_TipoRecursoFuenteFinanciamiento -> Cat_FuenteFinanciamiento")

        print("\n3. Conectando Dimensiones Faltantes al Presupuesto (Proyectos, Orientación, Asignación, etc.)...")
        cols = [
            ("ProyectoEstrategicoId", "Cat_ProyectoEstrategico", "fk_pc_pe"),
            ("OrientacionGastoId", "Cat_OrientacionGasto", "fk_pc_og"),
            ("AsignacionGastoId", "Cat_AsignacionGasto", "fk_pc_ag"),
            ("AnioGastoId", "Cat_AnioGasto", "fk_pc_anio"),
            ("ClasificacionAdministrativaId", "Cat_ClasificacionAdministrativa", "fk_pc_ca")
        ]
        for col, tabla, fk in cols:
            execute_sql(f"ALTER TABLE presupuestocalendarizado ADD COLUMN {col} INT", f"Añadir {col} a Presupuesto")
            execute_sql(f"ALTER TABLE presupuestocalendarizado ADD CONSTRAINT {fk} FOREIGN KEY ({col}) REFERENCES {tabla}(Id)", f"presupuestocalendarizado -> {tabla}")

        print("\n4. Conectando Módulos Operativos (Adecuaciones y Gastos)...")
        execute_sql("ALTER TABLE adecuaciones ADD COLUMN TipoAdecuacionId INT", "Añadir TipoAdecuacionId a Adecuaciones")
        execute_sql("ALTER TABLE adecuaciones ADD CONSTRAINT fk_adec_ta FOREIGN KEY (TipoAdecuacionId) REFERENCES Cat_TipoAdecuacion(Id)", "adecuaciones -> Cat_TipoAdecuacion")
        
        execute_sql("ALTER TABLE gastos ADD COLUMN TipoMinistracionId INT", "Añadir TipoMinistracionId a Gastos")
        execute_sql("ALTER TABLE gastos ADD CONSTRAINT fk_gas_tm FOREIGN KEY (TipoMinistracionId) REFERENCES Cat_TipoMinistracion(Id)", "gastos -> Cat_TipoMinistracion")

        print("\n5. Equilibrando Ecosistema (Creando Ingresos para conectar RubroIngresos)...")
        execute_sql("""
            CREATE TABLE IF NOT EXISTS ingresos_presupuesto (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                RubroIngresoId INT,
                Monto DECIMAL(18,2) DEFAULT 0.00,
                FechaRegistro DATETIME,
                CONSTRAINT fk_ing_ri FOREIGN KEY (RubroIngresoId) REFERENCES Cat_RubroIngresos(Id)
            )
        """, "Crear tabla ingresos_presupuesto y conectar con Cat_RubroIngresos")

        conn.commit()
        print("\n=======================================================")
        print("¡DIAGRAMA ERD UNIFICADO! Refresca tu DBeaver/Workbench")
        print("=======================================================")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'conn' in locals() and conn.is_connected():
            cursor.close()
            conn.close()

if __name__ == "__main__":
    unificar_erd()
