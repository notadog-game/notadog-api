namespace NotadogApi.Domain.Exceptions
{
    public enum ErrorCode
    {
        UserNotFound,
        UserEmailAlreadyExist,
        UserEmailUncorrect,
        UserPasswordTooShort,
        UserAlreadyInRoom,

        RoomStoragePlayerAlreadyInRoom,

        RoomPlayersMaxCountLacked,
        RoomNotInWaitingPlayersStateOnAddingPlayer,
        RoomOnAddingExistingPlayer,
        RoomStartingByNonRootPlayer,
        RoomStartingNotInWaitingPlayersState,
        RoomStartingNotEnoughPlayers,
        RoomReplayingdByNonRootPlayer,
        RoomReplayingNotInEndPlayersState,
        RoomMakeMoveNotInPlayingState,
    }
}
