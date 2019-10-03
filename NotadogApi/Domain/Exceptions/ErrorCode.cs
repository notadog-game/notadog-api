namespace NotadogApi.Domain.Exceptions
{
    public enum ErrorCode
    {
        UserNotFound,
        UserNameMustNotBeEmpty,
        UserEmailAlreadyExist,
        UserEmailMustNotBeEmpty,
        UserEmailIsNotValid,
        UserPasswordMustNotBeEmpty,
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
