
interface IInvokable {

    void CreateUnit(int targetX, int targetY, string unitType, int ownerId);
    void SplitUnit(int startX, int startY, int endX, int endY, int amount);
    void MoveToEmpty(int startX, int startY, int endX, int endY);
    void MoveToMerge(int startX, int startY, int endX, int endY);
    void MoveToAttack(int startX, int startY, int endX, int endY);

    void CashChanged(int playerId, int newValue);


}
