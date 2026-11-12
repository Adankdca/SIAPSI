import mysql.connector

def analizar_clave():
    conn = mysql.connector.connect(host='localhost', user='root', password='root', database='SIAP_DB')
    cur = conn.cursor()
    cur.execute("SELECT ClavePresupuestal FROM presupuestocalendarizado LIMIT 5")
    filas = cur.fetchall()
    
    print("CLAVES PRESUPUESTALES EN LA BD:")
    for f in filas:
        clave = f[0]
        print(f"[{len(clave)} chars] {clave}")
        
    cur.close()
    conn.close()

if __name__ == "__main__":
    analizar_clave()
