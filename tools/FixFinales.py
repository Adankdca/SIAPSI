import pandas as pd
import mysql.connector
import os
import re

def fix_finales():
    print("Conectando a MySQL para arreglar las últimas jerarquías (Programa Fondo y Municipio)...")
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        
        # -------------------------------------------------------------------
        # 1. MUNICIPIO (Añadir RegionDescripcion)
        # -------------------------------------------------------------------
        ruta_mun = os.path.join(base_dir, "13.-MUNICIPIOS.xls")
        if os.path.exists(ruta_mun):
            print("\n--- Repoblando MUNICIPIO con descripciones ---")
            try:
                cursor.execute("ALTER TABLE Cat_Municipio ADD COLUMN RegionDescripcion VARCHAR(255);")
            except: pass
            
            cursor.execute("TRUNCATE TABLE Cat_Municipio;")
            df = pd.read_excel(ruta_mun, header=None)
            
            current_region = ""
            current_region_desc = ""
            insertadas = 0
            
            for index, row in df.iterrows():
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if not vals: continue
                
                texto = vals[0]
                if len(vals) == 1 and re.match(r"^\d{3}\s+[A-Za-z]", texto):
                    partes = texto.split(" ", 1)
                    current_region = partes[0]
                    current_region_desc = partes[1] if len(partes) > 1 else ""
                    
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    if clave.isdigit() and len(clave) == 3 and desc.lower() != "descripción":
                        cursor.execute(
                            "INSERT INTO Cat_Municipio (RegionClave, RegionDescripcion, Clave, Nombre) VALUES (%s, %s, %s, %s)",
                            (current_region, current_region_desc, clave, desc)
                        )
                        insertadas += 1
            print(f"  [OK] Se reinsertaron {insertadas} Municipios con la descripción de su Región.")

        # -------------------------------------------------------------------
        # 2. PROGRAMA FONDO (Añadir TrDescripcion, FfDescripcion, RamoDescripcion)
        # -------------------------------------------------------------------
        ruta_pf = os.path.join(base_dir, "24.-PROGRAMA FONDO.xls")
        if os.path.exists(ruta_pf):
            print("\n--- Repoblando PROGRAMA FONDO con descripciones ---")
            columnas = ["TrDescripcion", "FfDescripcion", "RamoDescripcion"]
            for col in columnas:
                try: cursor.execute(f"ALTER TABLE Cat_ProgramaFondo ADD COLUMN {col} VARCHAR(255);")
                except: pass
            
            cursor.execute("TRUNCATE TABLE Cat_ProgramaFondo;")
            df = pd.read_excel(ruta_pf, header=None)
            
            current_tr = ""
            current_tr_desc = ""
            current_ff = ""
            current_ff_desc = ""
            current_ramo = ""
            current_ramo_desc = ""
            insertadas = 0
            
            for index, row in df.iterrows():
                vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
                if not vals: continue
                
                texto = vals[0]
                partes = texto.split(" ", 1)
                part = partes[0]
                desc_padre = partes[1] if len(partes) > 1 else ""
                
                # TR (1 dígito)
                if len(part) == 1 and len(vals) == 1:
                    current_tr = part
                    current_tr_desc = desc_padre
                # FF (2 dígitos)
                elif len(part) == 2 and len(vals) == 1:
                    current_ff = part
                    current_ff_desc = desc_padre
                # Ramo (4 caracteres alfanumericos, ej: 11A1)
                elif len(part) == 4 and len(vals) == 1:
                    current_ramo = part
                    current_ramo_desc = desc_padre
                    
                if len(vals) >= 2:
                    clave = vals[-2]
                    desc = vals[-1]
                    if len(clave) >= 4 and desc.lower() != "descripción":
                        cursor.execute("""
                            INSERT INTO Cat_ProgramaFondo 
                            (TrClave, TrDescripcion, FfClave, FfDescripcion, RamoClave, RamoDescripcion, Clave, Descripcion) 
                            VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
                            """, 
                            (current_tr, current_tr_desc, current_ff, current_ff_desc, current_ramo, current_ramo_desc, clave, desc))
                        insertadas += 1
            print(f"  [OK] Se reinsertaron {insertadas} Programas Fondo con la descripción completa de TR, FF y Ramo.")

        connection.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print("\n¡Corrección final completada!")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    fix_finales()
