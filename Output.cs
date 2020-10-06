namespace RandomFiles
{
    class Output
    {
        public void Show(string msg)
        {
            System.Console.WriteLine( msg );
        }

        public void Error(string msg)
        {
            System.Console.WriteLine( $"Error: {msg}" );
        }

        public void Warn(string msg)
        {
            System.Console.WriteLine( $"Warning: {msg}" );
        }


    }
}
