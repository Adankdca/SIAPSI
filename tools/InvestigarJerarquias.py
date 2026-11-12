import pandas as pd
import os

def investigar_jerarquias():
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    report_file = "jerarquias_report.txt"
    
    with open(report_file, "w", encoding="utf-8") as f:
        f.write("REPORTE DE INVESTIGACION DE JERARQUIAS\n")
        f.write("="*50 + "\n\n")
        
        for file in sorted(os.listdir(base_dir)):
            if not (file.endswith(".xls") or file.endswith(".xlsx")):
                continue
                
            ruta = os.path.join(base_dir, file)
            f.write(f"--- Archivo: {file} ---\n")
            
            try:
                df = pd.read_excel(ruta, header=None)
                # Tomamos las primeras 20 filas para ver la estructura
                texto = df.head(20).to_string(index=False, na_rep="")
                f.write(texto + "\n\n")
            except Exception as e:
                f.write(f"Error al leer: {e}\n\n")

if __name__ == "__main__":
    investigar_jerarquias()
