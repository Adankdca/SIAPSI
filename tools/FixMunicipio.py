import pandas as pd
import mysql.connector
import os
import re

def fix_municipio():
    print("Conectando a MySQL para corregir Municipio y añadir su Región...")
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    ruta_mun = os.path.join(base_dir, "13.-MUNICIPIOS.xls")
    
    if not os.path.exists(ruta_mun):
        print(f"Error: No se encontró {ruta_mun}")
        return
        
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        
        # 1. Modificar la tabla
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        try:
            cursor.execute("ALTER TABLE Cat_Municipio ADD COLUMN RegionClave VARCHAR(10);")
            print("Columna RegionClave añadida a la tabla Cat_Municipio.")
        except:
            pass # Ya existe
            
        cursor.execute("TRUNCATE TABLE Cat_Municipio;")
        
        # 2. Leer Excel y repoblar
        print("Leyendo Excel de Municipios...")
        df = pd.read_excel(ruta_mun, header=None)
        
        current_region = ""
        insertadas = 0
        
        for index, row in df.iterrows():
            vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
            if not vals: continue
            
            texto = vals[0]
            # Si tiene 1 valor y empieza con 3 digitos (ej. "901 Metropolitana")
            if len(vals) == 1 and re.match(r"^\d{3}\s+[A-Za-z]", texto):
                current_region = texto.split(" ")[0] # Se queda con "901"
                
            # Si tiene 2 o más valores, es Municipio
            if len(vals) >= 2:
                clave = vals[-2]
                desc = vals[-1]
                if clave.isdigit() and len(clave) == 3 and desc.lower() != "descripción":
                    cursor.execute(
                        "INSERT INTO Cat_Municipio (RegionClave, Clave, Nombre) VALUES (%s, %s, %s)",
                        (current_region, clave, desc)
                    )
                    insertadas += 1
                    
        connection.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print(f"¡Éxito! Se reinsertaron {insertadas} Municipios con su respectiva Región ({current_region} como última región leída).")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    fix_municipio()
