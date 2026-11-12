import mysql.connector

def normalizar():
    print("Iniciando Fase de Normalización de Base de Datos...")
    try:
        conn = mysql.connector.connect(host='localhost', database='SIAP_DB', user='root', password='root')
        cursor = conn.cursor()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        
        print("1. Normalizando Eje Org. Pub. -> UR -> URO...")
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS Cat_OrgPub (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Clave VARCHAR(20) UNIQUE,
                Descripcion VARCHAR(255)
            );
        """)
        cursor.execute("""
            INSERT IGNORE INTO Cat_OrgPub (Clave, Descripcion)
            SELECT DISTINCT OrgPubClave, OrgPubDescripcion FROM Cat_UnidadResponsableOperativa WHERE OrgPubClave != '';
        """)
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS Cat_UR (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                OrgPubId INT,
                Clave VARCHAR(20),
                Descripcion VARCHAR(255)
            );
        """)
        cursor.execute("TRUNCATE TABLE Cat_UR;")
        cursor.execute("""
            INSERT INTO Cat_UR (OrgPubId, Clave, Descripcion)
            SELECT DISTINCT op.Id, uro.UrClave, uro.UrDescripcion
            FROM Cat_UnidadResponsableOperativa uro
            JOIN Cat_OrgPub op ON op.Clave = uro.OrgPubClave
            WHERE uro.UrClave != '';
        """)
        try: cursor.execute("ALTER TABLE Cat_UnidadResponsableOperativa ADD COLUMN UrId INT;")
        except: pass
        cursor.execute("""
            UPDATE Cat_UnidadResponsableOperativa uro
            JOIN Cat_OrgPub op ON op.Clave = uro.OrgPubClave
            JOIN Cat_UR ur ON ur.Clave = uro.UrClave AND ur.OrgPubId = op.Id
            SET uro.UrId = ur.Id;
        """)

        print("2. Normalizando Eje Finalidad -> Función -> Subfunción -> Subsubfunción...")
        try: cursor.execute("ALTER TABLE Cat_Funcion ADD COLUMN FinalidadId INT;")
        except: pass
        cursor.execute("""
            UPDATE Cat_Funcion f
            JOIN (SELECT DISTINCT FinalidadClave, FuncionClave, FuncionDescripcion FROM Cat_Subfuncion) map 
              ON f.Clave = map.FuncionClave AND f.Descripcion = map.FuncionDescripcion
            JOIN Cat_Finalidad fin ON fin.Clave = map.FinalidadClave
            SET f.FinalidadId = fin.Id;
        """)
        cursor.execute("""
            UPDATE Cat_Funcion f
            JOIN (SELECT DISTINCT FinalidadClave, FuncionClave, FuncionDescripcion FROM Cat_Subsubfuncion) map 
              ON f.Clave = map.FuncionClave AND f.Descripcion = map.FuncionDescripcion
            JOIN Cat_Finalidad fin ON fin.Clave = map.FinalidadClave
            SET f.FinalidadId = fin.Id WHERE f.FinalidadId IS NULL;
        """)
        
        try: cursor.execute("ALTER TABLE Cat_Subfuncion ADD COLUMN FuncionId INT;")
        except: pass
        cursor.execute("""
            UPDATE Cat_Subfuncion sf
            JOIN Cat_Finalidad fin ON fin.Clave = sf.FinalidadClave
            JOIN Cat_Funcion f ON f.Clave = sf.FuncionClave AND f.Descripcion = sf.FuncionDescripcion AND f.FinalidadId = fin.Id
            SET sf.FuncionId = f.Id;
        """)
        
        try: cursor.execute("ALTER TABLE Cat_Subsubfuncion ADD COLUMN SubfuncionId INT;")
        except: pass
        cursor.execute("""
            UPDATE Cat_Subsubfuncion ssf
            JOIN Cat_Finalidad fin ON fin.Clave = ssf.FinalidadClave
            JOIN Cat_Funcion f ON f.Clave = ssf.FuncionClave AND f.Descripcion = ssf.FuncionDescripcion AND f.FinalidadId = fin.Id
            JOIN Cat_Subfuncion sf ON sf.Clave = ssf.SubfuncionClave AND sf.FuncionId = f.Id
            SET ssf.SubfuncionId = sf.Id;
        """)

        print("3. Normalizando Programa Fondo...")
        cols_pf = ["TipoRecursoId", "FuenteFinanciamientoId", "RamoId"]
        for c in cols_pf:
            try: cursor.execute(f"ALTER TABLE Cat_ProgramaFondo ADD COLUMN {c} INT;")
            except: pass
        cursor.execute("UPDATE Cat_ProgramaFondo pf JOIN Cat_TipoRecurso tr ON tr.Clave = pf.TrClave SET pf.TipoRecursoId = tr.Id;")
        cursor.execute("UPDATE Cat_ProgramaFondo pf JOIN Cat_FuenteFinanciamiento ff ON ff.Clave = pf.FfClave SET pf.FuenteFinanciamientoId = ff.Id;")
        cursor.execute("UPDATE Cat_ProgramaFondo pf JOIN Cat_Ramo r ON r.Clave = pf.RamoClave SET pf.RamoId = r.Id;")

        print("4. Normalizando Municipio...")
        try: cursor.execute("ALTER TABLE Cat_Municipio ADD COLUMN RegionId INT;")
        except: pass
        cursor.execute("UPDATE Cat_Municipio m JOIN Cat_Region r ON r.Clave = m.RegionClave SET m.RegionId = r.Id;")

        print("5. Limpieza Final de Redundancias...")
        limpiezas = [
            "ALTER TABLE Cat_UnidadResponsableOperativa DROP COLUMN OrgPubClave, DROP COLUMN OrgPubDescripcion, DROP COLUMN UrClave, DROP COLUMN UrDescripcion;",
            "ALTER TABLE Cat_Funcion DROP COLUMN FinalidadClave;",
            "ALTER TABLE Cat_Subfuncion DROP COLUMN FinalidadClave, DROP COLUMN FinalidadDescripcion, DROP COLUMN FuncionClave, DROP COLUMN FuncionDescripcion;",
            "ALTER TABLE Cat_Subsubfuncion DROP COLUMN FinalidadClave, DROP COLUMN FinalidadDescripcion, DROP COLUMN FuncionClave, DROP COLUMN FuncionDescripcion, DROP COLUMN SubfuncionClave;",
            "ALTER TABLE Cat_ProgramaFondo DROP COLUMN TrClave, DROP COLUMN TrDescripcion, DROP COLUMN FfClave, DROP COLUMN FfDescripcion, DROP COLUMN RamoClave, DROP COLUMN RamoDescripcion;",
            "ALTER TABLE Cat_Municipio DROP COLUMN RegionClave, DROP COLUMN RegionDescripcion;"
        ]
        for q in limpiezas:
            try: cursor.execute(q)
            except Exception as e: pass

        conn.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print("=========================================")
        print("¡BASE DE DATOS NORMALIZADA AL 100%!")
        print("=========================================")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'conn' in locals() and conn.is_connected():
            cursor.close()
            conn.close()

if __name__ == "__main__":
    normalizar()
