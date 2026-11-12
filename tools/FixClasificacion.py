import pandas as pd
import mysql.connector
import os

def fix_clasificacion():
    print("Conectando a MySQL para arreglar Clasificación Administrativa...")
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        
        # 1. Modificar la estructura de la tabla
        print("Actualizando estructura de la tabla...")
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        
        # Intentamos borrar el indice si existe, si no, lo ignoramos
        try:
            cursor.execute("ALTER TABLE Cat_ClasificacionAdministrativa DROP INDEX Clave;")
        except:
            pass
            
        # Añadir las 5 columnas de jerarquía (Ignoramos si ya existen)
        columnas = ["Spco", "Spnfy", "Se", "Sse", "Ssge"]
        for col in columnas:
            try:
                cursor.execute(f"ALTER TABLE Cat_ClasificacionAdministrativa ADD COLUMN {col} VARCHAR(5);")
            except:
                pass
                
        cursor.execute("TRUNCATE TABLE Cat_ClasificacionAdministrativa;")
        print("Tabla lista para recibir la jerarquía completa.")

        # 2. Leer el Excel y extraer las 7 columnas
        ruta = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)\1.-CLASIFICACION ADMINISTRATIVA.xls"
        if not os.path.exists(ruta):
            print(f"Error: No se encontró {ruta}")
            return
            
        print("Leyendo Excel...")
        df = pd.read_excel(ruta, header=None)
        
        insert_query = """
            INSERT INTO Cat_ClasificacionAdministrativa 
            (Spco, Spnfy, Se, Sse, Ssge, Clave, Descripcion) 
            VALUES (%s, %s, %s, %s, %s, %s, %s)
        """
        
        registros_insertados = 0
        for index, row in df.iterrows():
            vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
            
            # Buscamos renglones que tengan al menos 7 valores (las 5 jerarquias + OE + Descripcion)
            if len(vals) >= 7:
                spco = vals[-7]
                spnfy = vals[-6]
                se = vals[-5]
                sse = vals[-4]
                ssge = vals[-3]
                oe = vals[-2] # La Clave
                desc = vals[-1] # La Descripcion
                
                # Validamos que los primeros 5 sean dígitos (para no insertar cabeceras)
                if spco.isdigit() and spnfy.isdigit() and se.isdigit():
                    cursor.execute(insert_query, (spco, spnfy, se, sse, ssge, oe, desc))
                    registros_insertados += 1
                    
        connection.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print(f"¡Éxito! Se reinsertaron {registros_insertados} registros completos con su jerarquía SPCO-SPNFy-SE-SSE-SSGE.")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    fix_clasificacion()
