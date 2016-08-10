using DDD.Infra.Base;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DDD.Web.Helpers
{
    public static class ControllerError
    {
        public static void Processing(this Controller controller, Exception ex, bool clearModelState = true)
        {
            if (clearModelState)
            {
                controller.ModelState.Clear();
            }

            var appException = ex as AppException;
            if (appException != null && appException.ValidationErrors != null)
            {
                foreach (var erro in appException.ValidationErrors)
                    controller.ModelState.AddModelError(erro.MemberNames, erro.ErrorMessage);
            }


            //Valida lista não ordenada.
            string[] guids = null;

            var indexes = controller.Request.Form.AllKeys.Where(c => c.Contains(".Index"));

            foreach (var key in indexes)
            {
                string listName = key.Replace(".Index", string.Empty);
                guids = controller.Request.Form[key].Split(',');

                if (guids != null && guids.Count() > 0)
                {
                    if (appException != null && appException.ValidationErrors != null)
                    {
                        foreach (var erro in appException.ValidationErrors)
                        {
                            if (erro.MemberNames.Contains(string.Format("{0}[", listName)))
                            {
                                int i = int.Parse(erro.MemberNames.Substring(erro.MemberNames.IndexOf("[") + 1, 1));

                                string newErroMember = erro.MemberNames.Replace(string.Format("[{0}]", i), string.Format("[{0}]", guids[i]));
                                controller.ModelState.AddModelError(newErroMember, erro.ErrorMessage);
                            }
                        }
                    }
                }
            }

            controller.ViewBag.AlertError = ex.Message;
        }

        public static string Tratamento(Exception ex)
        {
            var appException = ex as AppException;
            string mensagemErro = string.Empty;
            if (appException != null && appException.ValidationErrors != null)
            {
                mensagemErro = string.Join("\n", appException.ValidationErrors.Select(x => x.ErrorMessage));
            }
            else
            {
                mensagemErro = ex.Message;
            }

            return mensagemErro;
        }
    }
}