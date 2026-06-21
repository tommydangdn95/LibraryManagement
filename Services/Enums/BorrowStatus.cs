namespace Services.Enums
{
    public enum BorrowStatus
    {
        Cancel = -1,
        SubmitRequest = 0,
        Approved = 1,
        Borrowing = 2,
        Returned = 3,
        Overdue = 4,
        Lost = 5
    }
}
