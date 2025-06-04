namespace ProjectPipeline.Core.Enums
{
    /// <summary>
    /// Comprehensive profile status enumeration
    /// </summary>
    public enum ProfileStatusEnum
    {
        Submitted = 1,
        UnderScreening = 2,
        Shortlisted = 3,
        InterviewScheduled = 4,
        InterviewCompleted = 5,
        TechnicalRoundScheduled = 6,
        TechnicalRoundCompleted = 7,
        Selected = 8,
        Rejected = 9,
        OnHold = 10,
        OfferExtended = 11,
        OfferAccepted = 12,
        OfferDeclined = 13,
        Joined = 14,
        DroppedOut = 15
    }
}
