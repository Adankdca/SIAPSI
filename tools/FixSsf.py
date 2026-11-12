import pandas as pd
import mysql.connector
import os

def fix_ssf():
    print("Conectando a MySQL para añadir descripciones a Subsubfunción...")
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    ruta_ssf = os.path.join(base_dir, "7.-SUBSUBFUNCION.xls")
    
    if not os.path.exists(ruta_ssf):
        print(f"Error: No se encontró {ruta_ssf}")
        return
        
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        
        # 1. Modificar la tabla
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        columnas = ["FinalidadDescripcion", "FuncionDescripcion", "SubfuncionDescripcion"]
        for col in columnas:
            try:
                cursor.execute(f"ALTER TABLE Cat_Subsubfuncion ADD COLUMN {col} VARCHAR(255);")
            except:
                pass # Ya existen
            
        cursor.execute("TRUNCATE TABLE Cat_Subsubfuncion;")
        
        # 2. Leer Excel y repoblar
        print("Leyendo Excel de SUBSUBFUNCION...")
        df = pd.read_excel(ruta_ssf, header=None)
        
        current_fin = ""
        current_fin_desc = ""
        current_fun = ""
        current_fun_desc = ""
        current_sfun = ""
        current_sfun_desc = ""
        insertadas = 0
        
        for index, row in df.iterrows():
            row_list = list(row.values)
            
            # Checamos Finalidad (Col 0)
            if not pd.isna(row_list[0]) and str(row_list[0]).strip() != '' and str(row_list[0]).lower() != 'finalidad':
                partes = str(row_list[0]).strip().split(" ", 1)
                current_fin = partes[0]
                current_fin_desc = partes[1] if len(partes) > 1 else ""
            
            # Checamos Funcion (Col 1)
            if len(row_list) > 1 and not pd.isna(row_list[1]) and str(row_list[1]).strip() != '' and str(row_list[1]).lower() != 'función':
                partes = str(row_list[1]).strip().split(" ", 1)
                current_fun = partes[0]
                current_fun_desc = partes[1] if len(partes) > 1 else ""
                
            # Checamos Subfuncion (Col 2)
            if len(row_list) > 2 and not pd.isna(row_list[2]) and str(row_list[2]).strip() != '' and str(row_list[2]).lower() != 'subfunción':
                partes = str(row_list[2]).strip().split(" ", 1)
                current_sfun = partes[0]
                current_sfun_desc = partes[1] if len(partes) > 1 else ""
                
            # Extraemos clave y desc de los ultimos valores no nulos
            vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
            if len(vals) >= 2:
                clave = vals[-2]
                desc = vals[-1]
                if clave.isdigit() and len(desc) > 3 and clave.lower() != "clave":
                    cursor.execute("""
                        INSERT INTO Cat_Subsubfuncion 
                        (FinalidadClave, FinalidadDescripcion, FuncionClave, FuncionDescripcion, SubfuncionClave, SubfuncionDescripcion, Clave, Descripcion) 
                        VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
                        """, 
                        (current_fin, current_fin_desc, current_fun, current_fun_desc, current_sfun, current_sfun_desc, clave, desc))
                    insertadas += 1
                    
        connection.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print(f"¡Éxito! Se reinsertaron {insertadas} Subsubfunciones con las descripciones de todos sus padres.")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    fix_ssf()
