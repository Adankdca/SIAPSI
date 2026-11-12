import mysql.connector

def run_alters():
    print("Conectando a MySQL para actualizar la estructura de las tablas...")
    try:
        connection = mysql.connector.connect(
            host='localhost',
            database='SIAP_DB',
            user='root',
            password='root'
        )
        cursor = connection.cursor()
        
        # Apagamos las llaves foraneas temporalmente
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0;")
        
        # Diccionario con las alteraciones
        alters = [
            # URO depende de UR
            "ALTER TABLE Cat_UnidadResponsableOperativa DROP INDEX Clave;",
            "ALTER TABLE Cat_UnidadResponsableOperativa ADD COLUMN UrClave VARCHAR(10);",
            
            # Funcion depende de Finalidad
            "ALTER TABLE Cat_Funcion DROP INDEX Clave;",
            "ALTER TABLE Cat_Funcion ADD COLUMN FinalidadClave VARCHAR(10);",
            
            # Subfuncion depende de Funcion (y Finalidad)
            "ALTER TABLE Cat_Subfuncion DROP INDEX Clave;",
            "ALTER TABLE Cat_Subfuncion ADD COLUMN FinalidadClave VARCHAR(10);",
            "ALTER TABLE Cat_Subfuncion ADD COLUMN FuncionClave VARCHAR(10);",
            
            # Subsubfuncion depende de Subfuncion
            "ALTER TABLE Cat_Subsubfuncion DROP INDEX Clave;",
            "ALTER TABLE Cat_Subsubfuncion ADD COLUMN FinalidadClave VARCHAR(10);",
            "ALTER TABLE Cat_Subsubfuncion ADD COLUMN FuncionClave VARCHAR(10);",
            "ALTER TABLE Cat_Subsubfuncion ADD COLUMN SubfuncionClave VARCHAR(10);",
            
            # Programa Fondo depende de TR, FF y Ramo
            "ALTER TABLE Cat_ProgramaFondo DROP INDEX Clave;",
            "ALTER TABLE Cat_ProgramaFondo ADD COLUMN TrClave VARCHAR(10);",
            "ALTER TABLE Cat_ProgramaFondo ADD COLUMN FfClave VARCHAR(10);",
            "ALTER TABLE Cat_ProgramaFondo ADD COLUMN RamoClave VARCHAR(10);"
        ]
        
        for q in alters:
            try:
                cursor.execute(q)
                print(f"[OK] {q}")
            except Exception as e:
                print(f"[AVISO] Ignorando (probablemente ya modificado): {q} -> {e}")
                
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1;")
        connection.commit()
        print("¡Base de datos actualizada con éxito!")
        
    except Exception as e:
        print(f"Error crítico: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    run_alters()
