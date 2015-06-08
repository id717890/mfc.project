using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLogger = NLog.Logger;

namespace mfc.infrastructure.services {
    public class Logger : ILogger {
        #region Fields

        readonly NLogger _logger;

        #endregion

        public Logger(NLogger log) {
            _logger = log;
        }

        public void Info(string message) {
            var obj = HttpContext.Current.Session["SessionScope"];

            _logger.Info(string.Format("user: [id: {3}] [{0} {1}] {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message, obj != null ? obj.GetHashCode() : -1));
        }

        public void Error(string message, Exception e) {
            _logger.ErrorException(string.Format("user: [{0} {1}] message {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message), e);
        }

        public void Error(Exception e) {
            Error("", e);
        }

        public void Fatal(string message, Exception e) {
            _logger.FatalException(string.Format("user: [{0} {1}] message {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message), e);
        }

        public void Fatal(string message) {
            _logger.Fatal(string.Format("user: [{0} {1}] {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message));
        }

        public void Debug(string message) {
            _logger.Debug(string.Format("user: [{0} {1}] {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message));
        }


        public void Error(string message) {
            _logger.Error(string.Format("user: [{0} {1}] {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message));
        }


        public void Warn(string message) {
            _logger.Warn(string.Format("user: [{0} {1}] {2}", HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress, message));
        }
    }
}
