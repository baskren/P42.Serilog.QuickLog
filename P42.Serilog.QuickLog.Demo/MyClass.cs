using P42.Serilog.QuickLog;

namespace MyApp
{
    public class MyClass
    {

        public void MyMethod()
        {
            try
            {
                throw new InvalidTimeZoneException("Bonkers!");
            }
            catch (Exception ex)
            {
                QLog.Error(this, ex);
            }

        }
    }
}