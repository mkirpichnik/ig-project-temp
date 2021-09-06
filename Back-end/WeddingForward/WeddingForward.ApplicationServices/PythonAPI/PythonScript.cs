namespace WeddingForward.ApplicationServices.PythonAPI
{
    public class PythonScript<TResult> where TResult: class
    {
        public string Path { get; set; }

        public bool IsAuthRequired { get; set; } = true;
    }

    // pyinstaller --onefile --windowed script-name.py
}
