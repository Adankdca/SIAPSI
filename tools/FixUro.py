import pandas as pd
import mysql.connector
import os

def fix_uro():
    print("Conectando a MySQL para añadir Org. Pub. a la Unidad Responsable Operativa...")
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    ruta_uro = os.path.join(base_dir, "3.-UNIDAD RESPONSABLE OPERATIVA.xls")
    
    if not os.path.exists(ruta_uro):
        print(f"Error: No se encontró {ruta_uro}")
        return
        
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        
        # 1. Modificar la tabla
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        try:
            cursor.execute("ALTER TABLE Cat_UnidadResponsableOperativa ADD COLUMN OrgPubClave VARCHAR(20);")
        except:
            pass # Ya existe
        try:
            cursor.execute("ALTER TABLE Cat_UnidadResponsableOperativa ADD COLUMN OrgPubDescripcion VARCHAR(255);")
            cursor.execute("ALTER TABLE Cat_UnidadResponsableOperativa ADD COLUMN UrDescripcion VARCHAR(255);")
        except:
            pass # Ya existen
            
        cursor.execute("TRUNCATE TABLE Cat_UnidadResponsableOperativa;")
        
        # 2. Leer Excel y repoblar
        print("Leyendo Excel de URO...")
        df = pd.read_excel(ruta_uro, header=None)
        
        current_orgpub = ""
        current_orgpub_desc = ""
        current_ur = ""
        current_ur_desc = ""
        insertadas = 0
        
        for index, row in df.iterrows():
            vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
            if not vals: continue
            
            texto = vals[0]
            partes = texto.split(" ", 1)
            part = partes[0]
            desc_padre = partes[1] if len(partes) > 1 else ""
            
            # Detectar Org Pub (8 dígitos, ej: 21111101)
            if part.isdigit() and len(part) == 8 and len(vals) == 1:
                current_orgpub = part
                current_orgpub_desc = desc_padre
                
            # Detectar UR (2 dígitos, ej: 01)
            elif part.isdigit() and len(part) == 2 and len(vals) == 1:
                current_ur = part
                current_ur_desc = desc_padre
                
            # Detectar URO (Clave 3 dígitos + Descripcion)
            if len(vals) >= 2:
                clave = vals[-2]
                desc = vals[-1]
                if clave.isdigit() and len(clave) == 3 and desc.lower() != "descripción":
                    cursor.execute(
                        "INSERT INTO Cat_UnidadResponsableOperativa (OrgPubClave, OrgPubDescripcion, UrClave, UrDescripcion, Clave, Descripcion) VALUES (%s, %s, %s, %s, %s, %s)",
                        (current_orgpub, current_orgpub_desc, current_ur, current_ur_desc, clave, desc)
                    )
                    insertadas += 1
                    
        connection.commit()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        print(f"¡Éxito! Se reinsertaron {insertadas} UROs con su jerarquía completa (Org Pub -> UR -> URO).")

    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    fix_uro()
