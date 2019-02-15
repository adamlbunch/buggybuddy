namespace buggybuddy.Repositories
{
    public class DataResponse<T>
    {
		public T Model { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
    }
}
