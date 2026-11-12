import pandas as pd
import os

files = [
    r"SISTEMA EXCEL\BOSQUEJO DE SISTEMA INTERNOI 2026.xlsx",
    r"SISTEMA EXCEL\INTEGRACION DE LA CLAVE Y CATALOOS.xlsx",
    r"SISTEMA EXCEL\PRESUPUESTOINICIAL.xlsx"
]

for file in files:
    print(f"\n=============================================")
    print(f"FILE: {file}")
    if not os.path.exists(file):
        print("FILE NOT FOUND")
        continue
    
    try:
        excel = pd.ExcelFile(file)
        print(f"SHEETS: {excel.sheet_names}")
        
        for sheet in excel.sheet_names:
            print(f"\n--- Sheet: {sheet} ---")
            try:
                # Read just the top 2 rows to get column names
                df = pd.read_excel(file, sheet_name=sheet, nrows=2)
                print(f"Columns ({len(df.columns)}): {list(df.columns)[:10]} ...")
            except Exception as e:
                print(f"Could not read sheet: {e}")
    except Exception as e:
        print(f"Could not open file: {e}")
