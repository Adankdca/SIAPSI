import mysql.connector

try:
    c = mysql.connector.connect(host='localhost', user='root', password='', database='siap')
    cur = c.cursor()
    cur.execute("SELECT Aprobado, Aumentos, Disminuciones, Modificado FROM presupuestocalendarizado WHERE ClavePresupuestal = '010012211H04501T11A001000A1B113110025115S28C001081120252'")
    row = cur.fetchone()
    print("Aprobado:", row[0])
    print("Aumentos:", row[1])
    print("Disminuciones:", row[2])
    print("Modificado:", row[3])
except Exception as e:
    print(e)
