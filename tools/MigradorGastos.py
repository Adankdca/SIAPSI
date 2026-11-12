import pandas as pd
import mysql.connector
from mysql.connector import Error

def migrate_gastos():
    print("Iniciando lectura de BASE GASTOS en el Excel...")
    file_path = r"SISTEMA EXCEL\BOSQUEJO DE SISTEMA INTERNOI 2026.xlsx"
    sheet = "BASE GASTOS"
    
    try:
        # Leemos el excel
        df = pd.read_excel(file_path, sheet_name=sheet)
        df.columns = [str(c).strip().upper() for c in df.columns]
        
        print(f"Excel leído correctamente. Se encontraron {len(df)} filas de Gastos.")
        print("Conectando a MySQL...")
        
        connection = mysql.connector.connect(
            host='localhost',
            database='SIAP_DB',
            user='root',
            password='root'
        )

        if connection.is_connected():
            cursor = connection.cursor(dictionary=True)
            
            # Limpiamos la tabla de gastos
            cursor.execute("TRUNCATE TABLE Gastos")
            
            # Precargar las Claves Presupuestales y sus IDs en un diccionario para búsqueda ultrarrápida
            print("Cargando catálogo de PresupuestoCalendarizado a memoria...")
            cursor.execute("SELECT Id, ClavePresupuestal FROM PresupuestoCalendarizado")
            presupuesto_map = {row['ClavePresupuestal'].strip(): row['Id'] for row in cursor.fetchall()}
            
            insert_query = """
                INSERT INTO Gastos (
                    PresupuestoCalendarizadoId, Folio, UnificaContrato, 
                    Beneficiario, Factura, ConceptoGasto, Ministracion, Importe, Estatus
                ) VALUES (
                    %s, %s, %s, %s, %s, %s, %s, %s, %s
                )
            """
            
            cursor_insert = connection.cursor()
            registros_insertados = 0
            registros_sin_presupuesto = 0
            
            print("Iniciando inserción masiva de Gastos...")
            for index, row in df.iterrows():
                
                # Función segura para extraer valores
                def get_val(col_name, is_numeric=False):
                    # Busca columna aproximada si no existe exacta
                    for col in df.columns:
                        if col_name in col:
                            val = row[col]
                            if pd.isna(val): return 0 if is_numeric else ""
                            return val
                    return 0 if is_numeric else ""
                
                clave_excel = str(get_val('CLAVE PRESUPUESTAL')).strip()
                if not clave_excel or clave_excel == '0':
                    continue
                    
                # Buscamos el ID del presupuesto (La llave foránea)
                presupuesto_id = presupuesto_map.get(clave_excel)
                
                if not presupuesto_id:
                    registros_sin_presupuesto += 1
                    continue # Si la clave no existe en el catálogo, no podemos insertar el Gasto por integridad referencial
                
                # Extraemos los datos usando palabras clave aproximadas
                folio = str(get_val('FOLIO'))
                contrato = str(get_val('CONTRATO'))
                beneficiario = str(get_val('BENEFICIARIO'))
                factura = str(get_val('FACTURA'))
                concepto = str(get_val('CONCEPTO'))
                ministracion = str(get_val('MINISTRACION'))
                importe = float(get_val('IMPORTE', True))
                estatus = str(get_val('ESTATUS'))
                
                if not estatus:
                    estatus = "Pendiente"
                
                data = (
                    presupuesto_id, folio, contrato, beneficiario, factura, concepto, ministracion, importe, estatus
                )
                
                cursor_insert.execute(insert_query, data)
                registros_insertados += 1
                
                if registros_insertados % 500 == 0:
                    connection.commit()
                    print(f"Insertados {registros_insertados} gastos...")
                    
            connection.commit()
            print(f"¡Éxito! Se migraron {registros_insertados} registros a la tabla Gastos.")
            if registros_sin_presupuesto > 0:
                print(f"Nota: Se omitieron {registros_sin_presupuesto} registros porque su Clave Presupuestal no existe en la tabla PresupuestoCalendarizado.")
            
    except Error as e:
        print(f"Error en MySQL: {e}")
    except Exception as e:
        print(f"Error procesando el archivo: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            cursor_insert.close()
            connection.close()
            print("Conexión MySQL cerrada.")

if __name__ == "__main__":
    migrate_gastos()
