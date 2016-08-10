using System;
using System.Collections.Generic;

namespace DDD.Domain.Base
{
    public class FeriadoHelper
    {
        public DateTime Data;
        public string Descricao;
        public List<FeriadoHelper> _feriados = new List<FeriadoHelper>();

        public FeriadoHelper(DateTime DataFeriado, string Descricao)
        {
            this.Data = DataFeriado;
            this.Descricao = Descricao;
        }


        /// <summary>
        /// INICIALIZA A CLASSE FERIADOS COM O ANO CORRENTE
        /// </summary>
        public FeriadoHelper()
        {
            int Ano = DateTime.Now.Year;
            GeraListaFeriados(Ano);
        }

        /// <summary>
        /// INICIALIZA A CLASSE FERIADOS COM O ANO DADO
        /// </summary>
        public FeriadoHelper(int Ano)
        {
            _DomingoPascoa = CalculaDiaPascoa(Ano);
            _DomingoCarnaval = CalculaDomingoCarnaval(_DomingoPascoa);
            _SegundaCarnaval = CalculaSegundaCarnaval(_DomingoPascoa);
            _TercaCarnaval = CalculaTercaCarnaval(_DomingoPascoa);
            _SextaPaixao = CalculaSextaPaixao(_DomingoPascoa);
            _CorpusChristi = CalculaCorpusChristi(_DomingoPascoa);
        }

        private void GeraListaFeriados(int ano)
        {
            var fm = new FeriadoHelper(ano);

            //ADICIONE AQUI OS FERIADOS PARA SUA CIDADE OU ESTADO SE NECESSÁRIO

            _feriados.Add(new FeriadoHelper(fm.DiaPascoa, "Domingo de Páscoa"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("01/01/" + ano), "Confraternização Universal"));
            _feriados.Add(new FeriadoHelper(fm.SegundaCarnaval, "Segunda Carnaval"));
            _feriados.Add(new FeriadoHelper(fm.TercaCarnaval, "Terça Carnaval"));
            _feriados.Add(new FeriadoHelper(fm.SextaPaixao, "Sexta feira da paixão"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("21/04/" + ano), "Tiradentes"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("01/05/" + ano), "Dia do trabalho"));
            _feriados.Add(new FeriadoHelper(fm.CorpusChristi, "Corpus Christi"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("07/09/" + ano), "Independência do Brasil"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("12/10/" + ano), "Padroeira do Brasil"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("02/11/" + ano), "Finados"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("15/11/" + ano), "Proclamação da República"));
            _feriados.Add(new FeriadoHelper(DateTime.Parse("25/12/" + ano), "Natal"));


        }

        public bool IsDiaUtil(DateTime data)
        {
            if (IsFimDeSemana(data) || IsFeriado(data))
                return false;
            else
                return true;
        }

        public bool IsFeriado(DateTime data)
        {
            return _feriados.Find(delegate (FeriadoHelper f1) { return f1.Data == data; }) != null ? true : false;
        }

        public bool IsFimDeSemana(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Sunday || data.DayOfWeek == DayOfWeek.Saturday)
                return true;
            else
                return false;
        }

        public DateTime ProximoDiaUtil(DateTime data)
        {
            DateTime auxData = data;

            if (IsFeriado(auxData) || IsFimDeSemana(auxData))
            {
                auxData = data.AddDays(1);
                auxData = ProximoDiaUtil(auxData);
            }

            return auxData;
        }

        public DateTime ObtenhaDataUtil(DateTime data, int quantidadeDiasUteis)
        {
            int cont = 0;

            while (true)
            {
                if (IsDiaUtil(data))
                {
                    cont++;
                }

                if (cont >= quantidadeDiasUteis)
                {
                    return data;
                }

                data = data.AddDays(1);

            }
        }


        #region  Private Fields
        private DateTime _DomingoPascoa;
        private DateTime _DomingoCarnaval;
        private DateTime _SegundaCarnaval;
        private DateTime _TercaCarnaval;
        private DateTime _SextaPaixao;
        private DateTime _CorpusChristi;
        #endregion

        #region Properties
        public DateTime DiaPascoa { get { return _DomingoPascoa; } }
        public DateTime DomingoCarnaval { get { return _DomingoCarnaval; } }
        public DateTime SegundaCarnaval { get { return _SegundaCarnaval; } }
        public DateTime TercaCarnaval { get { return _TercaCarnaval; } }
        public DateTime SextaPaixao { get { return _SextaPaixao; } }
        public DateTime CorpusChristi { get { return _CorpusChristi; } }
        #endregion

 
        /// <summary>
        ///  FUNÇÃO PARA CALCULAR A DATA DO DOMINGO DE PASCOA
        ///  DADO UM ANO QUALQUER
        /// </summary>
        /// <param name="AnoCalcular"></param>
        /// <returns></returns>
        private DateTime CalculaDiaPascoa(int AnoCalcular)
        {
            int x = 24;
            int y = 5;

            int a = AnoCalcular % 19;
            int b = AnoCalcular % 4;
            int c = AnoCalcular % 7;

            int d = (19 * a + x) % 30;
            int e = (2 * b + 4 * c + 6 * d + y) % 7;

            int dia = 0;
            int mes = 0;

            if (d + e > 9)
            {
                dia = (d + e - 9);
                mes = 4;
            }
            else
            {
                dia = (d + e + 22);
                mes = 3;
            }
            return DateTime.Parse(string.Format("{0},{1},{2}", AnoCalcular.ToString(), mes.ToString(), dia.ToString()));
        }


        private DateTime CalculaDomingoCarnaval(DateTime DataPascoa)
        {
            return DataPascoa.AddDays(-49);
        }

        private DateTime CalculaSegundaCarnaval(DateTime DataPascoa)
        {
            return DataPascoa.AddDays(-48);
        }

        private DateTime CalculaTercaCarnaval(DateTime DataPascoa)
        {
            return DataPascoa.AddDays(-47);
        }

        private DateTime CalculaSextaPaixao(DateTime DataPascoa)
        {
            return DataPascoa.AddDays(-2);
        }

        private DateTime CalculaCorpusChristi(DateTime DataPascoa)
        {
            return DataPascoa.AddDays(+60);
        }

    }
}

