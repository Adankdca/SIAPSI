import pandas as pd
import mysql.connector
from mysql.connector import Error
import math

def migrate_calendarizado():
    print("Iniciando lectura del archivo Excel (esto puede tomar unos segundos)...")
    file_path = r"SISTEMA EXCEL\CONCENTRADO DE ADECUACIONES 2025 VERTICAL.xls"
    sheet = "CALENDARIZADO INICIAL"
    
    try:
        # Leemos el excel
        df = pd.read_excel(file_path, sheet_name=sheet, header=4)
        
        # Limpiamos los nombres de columnas (mayúsculas y sin espacios)
        df.columns = [str(c).strip().upper() for c in df.columns]
        
        print(f"Excel leído correctamente. Se encontraron {len(df)} filas.")
        print("Conectando a MySQL...")
        
        connection = mysql.connector.connect(
            host='localhost',
            database='SIAP_DB',
            user='root',
            password='root'
        )

        if connection.is_connected():
            cursor = connection.cursor()
            
            # Limpiamos la tabla antes de insertar ignorando las restricciones de llave foránea temporalmente
            cursor.execute("SET FOREIGN_KEY_CHECKS = 0")
            cursor.execute("TRUNCATE TABLE PresupuestoCalendarizado")
            cursor.execute("SET FOREIGN_KEY_CHECKS = 1")
            
            # Preparamos la consulta de inserción (ajustamos a las columnas que existan en la tabla)
            insert_query = """
                INSERT INTO PresupuestoCalendarizado (
                    Secuencia, Marca, ClavePresupuestal,
                    Aprobado, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre
                ) VALUES (
                    %s, %s, %s,
                    %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s
                )
            """
            
            registros_insertados = 0
            
            print("Iniciando inserción masiva en MySQL...")
            for index, row in df.iterrows():
                # Extraemos la clave dinámica
                clave = ""
                for col_c in ['CLAVE PRESUPUESTAL', 'CLAVES', 'CLAVE']:
                    if col_c in df.columns and not pd.isna(row[col_c]):
                        clave = str(row[col_c]).strip()
                        break
                
                # Evitar filas vacías
                if not clave:
                    continue
                
                # Función segura para extraer valores de pandas previniendo NaN
                def get_val(col_name, is_numeric=False):
                    if col_name not in df.columns: return 0 if is_numeric else ""
                    val = row[col_name]
                    if pd.isna(val): return 0 if is_numeric else ""
                    return val
                
                # Mapeamos los datos del Excel a la consulta
                data = (
                    str(get_val('SEC')), str(get_val('MARCA')), clave,
                    float(get_val('APROBADO', True)), float(get_val('ENERO', True)), float(get_val('FEBRERO', True)), 
                    float(get_val('MARZO', True)), float(get_val('ABRIL', True)), float(get_val('MAYO', True)), 
                    float(get_val('JUNIO', True)), float(get_val('JULIO', True)), float(get_val('AGOSTO', True)), 
                    float(get_val('SEPTIEMBRE', True)), float(get_val('OCTUBRE', True)), float(get_val('NOVIEMBRE', True)), 
                    float(get_val('DICIEMBRE', True))
                )
                
                cursor.execute(insert_query, data)
                registros_insertados += 1
                
                # Commit cada 500 registros para no saturar memoria
                if registros_insertados % 500 == 0:
                    connection.commit()
                    print(f"Insertados {registros_insertados} registros...")
                    
            connection.commit() # Commit final
            print(f"¡Éxito! Se migraron un total de {registros_insertados} registros a MySQL.")
            
    except Error as e:
        print(f"Error en MySQL: {e}")
    except Exception as e:
        print(f"Error procesando el archivo: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()
            print("Conexión MySQL cerrada.")

if __name__ == "__main__":
    migrate_calendarizado()
