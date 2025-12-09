namespace TaskManagerAPI.Domain.Models.Dto
{
    public class ResultOperation
    {
        public bool stateOperation;
        public string MessageResult;
        public string MessageExceptionUser;
        public string MessageExceptionTechnical;
        public bool RollBack;
    }
    public class ResultOperation<T> : ResultOperation
    {
        public T? Result { get; set; }
        public List<T> Results { get; set; } = new();
    }
}
