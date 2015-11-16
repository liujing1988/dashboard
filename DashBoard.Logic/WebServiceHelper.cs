using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.Services.Protocols;  

namespace DashBoard.Logic
{
    public class WebServiceHelper
    {

        #region InvokeWebService

        public static object InvokeAndCallWebService(string url, string methodname, object[] args)
        {
            return WebServiceHelper.InvokeAndCallWebService(url, null, methodname, args);
        }
        public static object InvokeAndCallWebService(string url,/* string @namespace, */string classname, string methodname, object[] args = null)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                CookieContainer cookieContainer = new CookieContainer();
                string cookieDomain = new Uri(url).Host;
                foreach (string cookieKey in HttpContext.Current.Request.Cookies.AllKeys)
                {
                    System.Net.Cookie netCookie = new System.Net.Cookie(cookieKey, HttpContext.Current.Request.Cookies[cookieKey].Value, "/", cookieDomain);
                    netCookie.Expires = DateTime.Now.AddMinutes(5);
                    cookieContainer.Add(netCookie);
                } 
                HttpClient webClient = new HttpClient(cookieContainer);
                Stream stream = webClient.OpenRead(url + "?WSDL");
                ServiceDescription description = ServiceDescription.Read(stream);
                ServiceDescriptionImporter descriptionImporter = new ServiceDescriptionImporter();
                descriptionImporter.AddServiceDescription(description, "", "");
                CodeNamespace codeNamespace = new CodeNamespace(@namespace);

                //生成客户端代理类代码   
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(codeNamespace);
                descriptionImporter.Import(codeNamespace, codeCompileUnit);
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();

                //设定编译参数   
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateExecutable = false;
                compilerParameters.GenerateInMemory = true;
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.XML.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类   
                CompilerResults codeResult = codeProvider.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
                if (true == codeResult.Errors.HasErrors)
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError compileError in codeResult.Errors)
                    {
                        stringBuilder.Append(compileError.ToString());
                        stringBuilder.Append(System.Environment.NewLine);
                    }
                    throw new Exception(stringBuilder.ToString());
                }

                //生成代理实例，并调用方法   
                System.Reflection.Assembly assembly = codeResult.CompiledAssembly;
                Type @class = assembly.GetType(@namespace + "." + classname, true, true);
                object instance = Activator.CreateInstance(@class);
                System.Reflection.MethodInfo methorInfo = @class.GetMethod(methodname);

                return methorInfo.Invoke(instance, args);

            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }
}
