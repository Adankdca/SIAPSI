import pandas as pd
import mysql.connector
import os
import re

def fix_restantes():
    print("Conectando a MySQL para repoblar catálogos faltantes (URO, Función, Subsubfunción)...")
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        
        # -------------------------------------------------------------------
        # 1. FUNCION (Solo Clave y Descripcion, permitiendo duplicados)
        # -------------------------------------------------------------------
        ruta_fun = os.path.join(base_dir, "5.-FUNCION.xls")
        if os.path.exists(ruta_fun):
            print("\n--- Repoblando FUNCION ---")
            cursor.execute("TRUNCATE TABLE Cat_Funcion;")
            df = pd.read_excel(ruta_fun, header=None)
            
            insertadas = 0
            for index, row in df.iterrows():
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    if clave.isdigit() and len(desc) > 3 and clave.lower() != "clave":
                        # Insertamos todas, aunque se repita la clave (ya no hay UNIQUE)
                        cursor.execute("INSERT INTO Cat_Funcion (Clave, Descripcion) VALUES (%s, %s)", (clave, desc))
                        insertadas += 1
            print(f"  [OK] Se insertaron {insertadas} Funciones (incluyendo duplicados legítimos).")

        # -------------------------------------------------------------------
        # 2. URO (Depende de UR)
        # -------------------------------------------------------------------
        ruta_uro = os.path.join(base_dir, "3.-UNIDAD RESPONSABLE OPERATIVA.xls")
        if os.path.exists(ruta_uro):
            print("\n--- Repoblando URO ---")
            cursor.execute("TRUNCATE TABLE Cat_UnidadResponsableOperativa;")
            df = pd.read_excel(ruta_uro, header=None)
            
            current_ur = ""
            insertadas = 0
            for index, row in df.iterrows():
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if not vals: continue
                
                texto = vals[0]
                # Detectamos si es una UR (ej. "01 Secretaría Técnica")
                if len(texto.split(" ")[0]) == 2 and texto.split(" ")[0].isdigit() and len(vals) == 1:
                    current_ur = texto.split(" ")[0]
                    
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    # Las URO tienen 3 digitos
                    if len(clave) == 3 and clave.isdigit():
                        cursor.execute("INSERT INTO Cat_UnidadResponsableOperativa (UrClave, Clave, Descripcion) VALUES (%s, %s, %s)", 
                                       (current_ur, clave, desc))
                        insertadas += 1
            print(f"  [OK] Se insertaron {insertadas} UROs con su jerarquía UR.")

        # -------------------------------------------------------------------
        # 3. SUBSUBFUNCION (Depende de Finalidad, Funcion, Subfuncion)
        # -------------------------------------------------------------------
        ruta_ssf = os.path.join(base_dir, "7.-SUBSUBFUNCION.xls")
        if os.path.exists(ruta_ssf):
            print("\n--- Repoblando SUBSUBFUNCION ---")
            cursor.execute("TRUNCATE TABLE Cat_Subsubfuncion;")
            df = pd.read_excel(ruta_ssf, header=None)
            
            current_fin = ""
            current_fun = ""
            current_sfun = ""
            insertadas = 0
            
            for index, row in df.iterrows():
                # En pandas, la columna 0 es Finalidad, 1 es Funcion, 2 es Subfuncion, 4 es Clave SSF, 7 es Descripcion
                row_list = list(row.values)
                
                # Checamos Finalidad (Col 0)
                if not pd.isna(row_list[0]) and str(row_list[0]).strip() != '' and str(row_list[0]).lower() != 'finalidad':
                    current_fin = str(row_list[0]).split(" ")[0]
                
                # Checamos Funcion (Col 1)
                if len(row_list) > 1 and not pd.isna(row_list[1]) and str(row_list[1]).strip() != '' and str(row_list[1]).lower() != 'función':
                    current_fun = str(row_list[1]).split(" ")[0]
                    
                # Checamos Subfuncion (Col 2)
                if len(row_list) > 2 and not pd.isna(row_list[2]) and str(row_list[2]).strip() != '' and str(row_list[2]).lower() != 'subfunción':
                    current_sfun = str(row_list[2]).split(" ")[0]
                    
                # Extraemos clave y desc de los ultimos valores no nulos
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    if clave.isdigit() and len(desc) > 3 and clave.lower() != "clave":
                        cursor.execute("INSERT INTO Cat_Subsubfuncion (FinalidadClave, FuncionClave, SubfuncionClave, Clave, Descripcion) VALUES (%s, %s, %s, %s, %s)", 
                                       (current_fin, current_fun, current_sfun, clave, desc))
                        insertadas += 1
            print(f"  [OK] Se insertaron {insertadas} Subsubfunciones con su jerarquía completa.")

        connection.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print("\n¡Repoblación de catálogos restantes completada!")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    fix_restantes()
