namespace BooksIO2026.Service.Common
{
    public class Result<T> where T : class
    {
        public bool IsSuccess { get; }
        public bool Isfailure => !IsSuccess;
        public T? Value { get; set; }
        public List<string> Errors { get; set; } = [];//usamos collection expressions
        private Result(bool success, List<string> errors,T? value=null)
        {
            IsSuccess = success;
            Errors = errors;
            Value = value;
        }
        public static Result<T> Success(T value)
        {
            return new Result<T>(true, new List<string>(),value);
        }
        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T>(false, errors);
        }
        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, [error]);//para que sea mas limpio podemos usar collection expressions asi no poner List<string>(){error}
        }
    }
}
