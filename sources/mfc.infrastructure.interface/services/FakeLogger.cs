using System;

namespace mfc.infrastructure.services {
    public class FakeLogger : ILogger {
        public void Info(string message) {        
        }

        public void Error(string message, Exception e) {
        }

        public void Error(Exception e) {
        }

        public void Fatal(string message, Exception e) {
        }

        public void Fatal(string message) {
        }

        public void Debug(string message) {
        }


        public void Error(string message) {
        }


        public void Warn(string message) {
        }
    }
}
