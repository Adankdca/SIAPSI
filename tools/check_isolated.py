import mysql.connector

c = mysql.connector.connect(host='localhost',user='root',password='root',database='SIAP_DB')
cur = c.cursor()
cur.execute("SELECT TABLE_NAME, REFERENCED_TABLE_NAME FROM information_schema.KEY_COLUMN_USAGE WHERE TABLE_SCHEMA = 'SIAP_DB' AND REFERENCED_TABLE_NAME IS NOT NULL")
edges = cur.fetchall()

graph = {}
for t, r in edges:
    if t not in graph: graph[t] = []
    if r not in graph: graph[r] = []
    graph[t].append(r)
    graph[r].append(t)

visited = set()
q = ['presupuestocalendarizado']
while q:
    curr = q.pop()
    if curr not in visited:
        visited.add(curr)
        for n in graph.get(curr, []):
            if n not in visited:
                q.append(n)

cur.execute("SHOW TABLES")
all_tables = {r[0] for r in cur.fetchall()}
isolated = all_tables - visited
print("Isolated tables:", isolated)
