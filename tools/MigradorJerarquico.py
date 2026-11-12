import pandas as pd
import mysql.connector
from mysql.connector import Error
import os
import re

def migrate_hierarchical():
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    print("Iniciando Migración Jerárquica...")
    
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0")
        
        # -------------------------------------------------------------
        # 1. Programa Fondo (Depende de TR, FF, Ramo)
        # -------------------------------------------------------------
        print("\n--- Procesando PROGRAMA FONDO ---")
        ruta_pf = os.path.join(base_dir, "24.-PROGRAMA FONDO.xls")
        if os.path.exists(ruta_pf):
            df_pf = pd.read_excel(ruta_pf, header=None)
            
            # Para arrastrar (fill-forward) los valores
            current_tr = ""
            current_ff = ""
            current_ramo = ""
            
            cursor.execute("TRUNCATE TABLE Cat_ProgramaFondo")
            
            for index, row in df_pf.iterrows():
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if not vals: continue
                
                # Identificando jerarquías por longitud o contenido
                # TR suele ser "1 No Etiquetado", "2 Etiquetado"
                texto = vals[0]
                
                if re.match(r"^[1-2] [a-zA-Z]", texto) and len(vals) == 1:
                    current_tr = texto.split(" ")[0] # Extrae el '1' o '2'
                elif re.match(r"^[1-2][1-9] [a-zA-Z]", texto) and len(vals) == 1:
                    current_ff = texto.split(" ")[0] # Extrae '11', '12'
                elif re.match(r"^[1-2][1-9][A-Z][1-9] [a-zA-Z]", texto) and len(vals) == 1:
                    current_ramo = texto.split(" ")[0] # Extrae '11A1'
                
                # Si hay dos columnas, asumimos que es Clave y Descripcion
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    if len(clave) <= 10 and len(desc) > 3 and clave.lower() != "clave":
                        if any(c.isdigit() for c in clave) or clave.isalpha():
                            cursor.execute(
                                "INSERT INTO Cat_ProgramaFondo (TrClave, FfClave, RamoClave, Clave, Descripcion) VALUES (%s, %s, %s, %s, %s)",
                                (current_tr, current_ff, current_ramo, clave, desc)
                            )
            connection.commit()
            print("Programa Fondo insertado con jerarquía.")

        # -------------------------------------------------------------
        # 2. Subfuncion (Depende de Finalidad y Funcion)
        # -------------------------------------------------------------
        print("\n--- Procesando SUBFUNCION ---")
        ruta_sf = os.path.join(base_dir, "6.-SUBFUNCION.xls")
        if os.path.exists(ruta_sf):
            df_sf = pd.read_excel(ruta_sf, header=None)
            current_fin = ""
            current_fun = ""
            
            cursor.execute("TRUNCATE TABLE Cat_Subfuncion")
            for index, row in df_sf.iterrows():
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if not vals: continue
                texto = vals[0]
                
                if len(texto.split(" ")[0]) == 1 and len(vals) == 1:
                    current_fin = texto.split(" ")[0]
                elif len(texto.split(" ")[0]) == 2 and len(vals) == 1:
                    current_fun = texto.split(" ")[0]
                    
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    if len(clave) <= 10 and clave.isdigit() and len(desc) > 3:
                        cursor.execute(
                            "INSERT INTO Cat_Subfuncion (FinalidadClave, FuncionClave, Clave, Descripcion) VALUES (%s, %s, %s, %s)",
                            (current_fin, current_fun, clave, desc)
                        )
            connection.commit()
            print("Subfuncion insertada con jerarquía.")
            
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1")
        print("\n¡Catálogos Jerárquicos procesados exitosamente!")
            
    except Error as e:
        print(f"Error MySQL: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    migrate_hierarchical()
