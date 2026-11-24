using System;
using System.Collections.Generic;
using System.Linq;
using SIAP.Models;
using SIAP.Repositories;

namespace SIAP.Services
{
    /// <summary>
    /// Separa una clave presupuestal en sus segmentos según la estructura del ejercicio
    /// y busca cada valor en su catálogo, respetando los catálogos jerárquicos
    /// (UR dentro del organismo, URO dentro de su UR, Función dentro de su Finalidad, etc.).
    /// Los catálogos se leen una sola vez por instancia.
    /// </summary>
    public class ClavePresupuestalService
    {
        private readonly EstructuraClaveRepository _repo = new EstructuraClaveRepository();
        private readonly Dictionary<string, Dictionary<string, EstructuraClaveRepository.RegistroCatalogo>> _catalogos =
            new Dictionary<string, Dictionary<string, EstructuraClaveRepository.RegistroCatalogo>>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Dictionary<int, EstructuraClaveRepository.RegistroCatalogo>> _porId =
            new Dictionary<string, Dictionary<int, EstructuraClaveRepository.RegistroCatalogo>>(StringComparer.OrdinalIgnoreCase);
        // Por catálogo: claves que aparecen UNA sola vez en toda la tabla (sin importar el padre)
        private readonly Dictionary<string, Dictionary<string, EstructuraClaveRepository.RegistroCatalogo>> _unicos =
            new Dictionary<string, Dictionary<string, EstructuraClaveRepository.RegistroCatalogo>>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _catalogosInvalidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private bool _orgLeido;
        private int? _orgPubId;

        public const string SegmentoOrganismo = "ORGPUB";

        public int? OrgPubId
        {
            get
            {
                if (!_orgLeido) { _orgPubId = _repo.ObtenerOrgPubId(); _orgLeido = true; }
                return _orgPubId;
            }
        }

        /// <summary>Catálogos configurados que no existen o cuyas columnas no coinciden.</summary>
        public IEnumerable<string> CatalogosInvalidos { get { return _catalogosInvalidos; } }

        private static string Llave(string clave, int? padreId)
        {
            return (clave ?? "") + "|" + (padreId.HasValue ? padreId.Value.ToString() : "");
        }

        /// <summary>Identifica un catálogo en caché: tabla, columnas y, si se filtra por año, el año.</summary>
        private static string IdCatalogo(SegmentoEstructura seg)
        {
            string anio = string.IsNullOrWhiteSpace(seg.ColumnaAnio) ? "" : seg.ColumnaAnio + "=" + seg.Anio;
            return seg.TablaCatalogo + "|" + seg.ColumnaClave + "|" + seg.ColumnaPadre + "|" + anio;
        }

        /// <summary>
        /// Valor con el que se busca el segmento en su catálogo: el valor solo o, si la estructura
        /// indica SegmentosLlave (p. ej. TR,FF,RA), esos valores seguidos del valor del segmento.
        /// </summary>
        private static string ValorBusqueda(SegmentoValor sv, SegmentoEstructura seg, List<SegmentoValor> segmentos)
        {
            if (string.IsNullOrWhiteSpace(seg.SegmentosLlave)) return sv.Valor;
            var sb = new System.Text.StringBuilder();
            foreach (var abrev in seg.SegmentosLlave.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var otro = segmentos.FirstOrDefault(x => string.Equals(x.Abreviatura, abrev.Trim(), StringComparison.OrdinalIgnoreCase));
                sb.Append(otro == null ? "" : otro.Valor);
            }
            return sb.Append(sv.Valor).ToString();
        }

        private Dictionary<string, EstructuraClaveRepository.RegistroCatalogo> Catalogo(SegmentoEstructura seg)
        {
            string id = IdCatalogo(seg);
            Dictionary<string, EstructuraClaveRepository.RegistroCatalogo> idx;
            if (_catalogos.TryGetValue(id, out idx)) return idx;

            idx = new Dictionary<string, EstructuraClaveRepository.RegistroCatalogo>();
            var porId = new Dictionary<int, EstructuraClaveRepository.RegistroCatalogo>();
            var unicos = new Dictionary<string, EstructuraClaveRepository.RegistroCatalogo>(StringComparer.OrdinalIgnoreCase);
            var repetidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var registros = _repo.LeerCatalogo(seg.TablaCatalogo, seg.ColumnaClave, seg.ColumnaPadre,
                                               seg.ColumnaAnio, string.IsNullOrWhiteSpace(seg.ColumnaAnio) ? (int?)null : seg.Anio);
            if (registros == null)
            {
                _catalogosInvalidos.Add(seg.TablaCatalogo);
            }
            else
            {
                bool jerarquico = !string.IsNullOrWhiteSpace(seg.ColumnaPadre);
                foreach (var r in registros) // ordenados por Id: si se repite, gana el primero
                {
                    string k = Llave(r.Clave, jerarquico ? r.PadreId : null);
                    if (!idx.ContainsKey(k)) idx[k] = r;
                    porId[r.Id] = r;
                    string c = r.Clave ?? "";
                    if (repetidas.Contains(c)) continue;
                    if (unicos.ContainsKey(c)) { unicos.Remove(c); repetidas.Add(c); }
                    else unicos[c] = r;
                }
            }
            _catalogos[id] = idx;
            _porId[id] = porId;
            _unicos[id] = unicos;
            return idx;
        }

        /// <summary>Descripción de un registro del catálogo del segmento, por su Id. Null si no existe.</summary>
        public string DescripcionPorId(SegmentoEstructura seg, int catalogoId)
        {
            if (seg == null || string.IsNullOrWhiteSpace(seg.TablaCatalogo)) return null;
            Catalogo(seg);
            string id = IdCatalogo(seg);
            EstructuraClaveRepository.RegistroCatalogo r;
            return _porId[id].TryGetValue(catalogoId, out r) ? r.Descripcion : null;
        }

        /// <summary>
        /// Separa la clave según la estructura. No valida la longitud: si la clave es más corta,
        /// los segmentos que no alcanzan quedan fuera.
        /// </summary>
        public List<SegmentoValor> Separar(string clave, List<SegmentoEstructura> estructura)
        {
            var lista = new List<SegmentoValor>();
            foreach (var seg in estructura.OrderBy(s => s.Orden))
            {
                int ini = seg.Posicion - 1;
                if (ini < 0 || ini + seg.Longitud > clave.Length) break;
                lista.Add(new SegmentoValor
                {
                    Orden = seg.Orden,
                    Abreviatura = seg.Abreviatura,
                    Nombre = seg.Nombre,
                    Valor = clave.Substring(ini, seg.Longitud),
                    TablaCatalogo = string.IsNullOrWhiteSpace(seg.TablaCatalogo) ? null : seg.TablaCatalogo,
                    ColumnaFK = string.IsNullOrWhiteSpace(seg.ColumnaFK) ? null : seg.ColumnaFK
                });
            }
            return lista;
        }

        /// <summary>
        /// Busca cada segmento en su catálogo (en orden, para que el padre se resuelva antes que el hijo)
        /// y llena CatalogoId y Descripcion. Un valor que no existe queda con CatalogoId = null.
        /// </summary>
        public void Resolver(List<SegmentoValor> segmentos, List<SegmentoEstructura> estructura)
        {
            var porAbrev = estructura.ToDictionary(s => s.Abreviatura, StringComparer.OrdinalIgnoreCase);
            var resueltos = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase);

            foreach (var sv in segmentos.OrderBy(s => s.Orden))
            {
                SegmentoEstructura seg;
                if (!porAbrev.TryGetValue(sv.Abreviatura, out seg)) continue;

                if (seg.Abreviatura == "CP")
                {
                    sv.Descripcion = "Ejercicio " + sv.Valor;
                    resueltos[sv.Abreviatura] = null;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(seg.TablaCatalogo))
                {
                    resueltos[sv.Abreviatura] = null;
                    continue;
                }

                int? padreId = null;
                bool jerarquico = !string.IsNullOrWhiteSpace(seg.ColumnaPadre);
                if (jerarquico)
                {
                    if (string.Equals(seg.SegmentoPadre, SegmentoOrganismo, StringComparison.OrdinalIgnoreCase))
                        padreId = OrgPubId;
                    else if (!string.IsNullOrWhiteSpace(seg.SegmentoPadre))
                    {
                        int? p;
                        resueltos.TryGetValue(seg.SegmentoPadre, out p);
                        padreId = p;
                    }
                }

                var idx = Catalogo(seg);
                EstructuraClaveRepository.RegistroCatalogo reg = null;
                string valor = ValorBusqueda(sv, seg, segmentos);
                bool encontrado = (!jerarquico || padreId.HasValue) && idx.TryGetValue(Llave(valor, padreId), out reg);

                // Respaldo: el catálogo no tiene bien ligado el padre (p. ej. las partidas sin su OGC),
                // pero si la clave existe UNA sola vez en toda la tabla, no hay duda de cuál es.
                // No aplica a la UR: esa siempre debe pertenecer al organismo configurado.
                if (!encontrado && jerarquico
                    && !string.Equals(seg.SegmentoPadre, SegmentoOrganismo, StringComparison.OrdinalIgnoreCase))
                {
                    string idCat = IdCatalogo(seg);
                    Dictionary<string, EstructuraClaveRepository.RegistroCatalogo> unicos;
                    encontrado = _unicos.TryGetValue(idCat, out unicos) && unicos.TryGetValue(valor ?? "", out reg);
                }

                if (encontrado)
                {
                    sv.CatalogoId = reg.Id;
                    sv.Descripcion = reg.Descripcion;
                }
                resueltos[sv.Abreviatura] = sv.CatalogoId;
            }
        }

        /// <summary>
        /// Decodifica una clave sin saber el año: lo detecta por la longitud y el segmento CP.
        /// </summary>
        public ClaveDecodificadaDto Decodificar(string clave)
        {
            var res = new ClaveDecodificadaDto();
            if (string.IsNullOrWhiteSpace(clave)) return res;
            clave = clave.Trim();

            int? anio = _repo.DetectarAnio(clave);
            if (!anio.HasValue) return res;

            var estructura = _repo.ObtenerEstructura(anio.Value);
            var segmentos = Separar(clave, estructura);
            Resolver(segmentos, estructura);
            foreach (var s in segmentos)
            {
                res.Segmentos.Add(new SegmentoClaveDto
                {
                    Nombre = s.Nombre,
                    Abreviatura = s.Abreviatura,
                    Valor = s.Valor,
                    Descripcion = s.Descripcion ?? (s.TablaCatalogo == null ? "" : "No existe en el catálogo")
                });
            }
            return res;
        }
    }
}
