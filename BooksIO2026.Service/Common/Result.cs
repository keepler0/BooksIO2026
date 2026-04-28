namespace BooksIO2026.Service.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool Isfailure => !IsSuccess;
        public List<string> Errors { get; set; } = [];//usamos collection expressions
        private Result(bool success, List<string> errors)
        {
            IsSuccess = success;
            Errors = errors;
        }
        public static Result Success()
        {
            return new Result(true, new List<string>());
        }
        public static Result Failure(List<string> errors)
        {
            return new Result(false, errors);
        }
        public static Result Failure(string error)
        {
            return new Result(false, [error]);//para que sea mas limpio podemos usar collection expressions asi no poner List<string>(){error}
        }
    }
}
