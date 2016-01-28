﻿
interface IInvokable {

    void CreateUnit(int targetX, int targetY, string unitType, int ownerId);

    void MoveToEmpty(int startX, int startY, int endX, int endY);
    void MoveToMerge(int startX, int startY, int endX, int endY);
    void MoveToAttack(int startX, int startY, int endX, int endY);

    void CashChanged(int playerId, int newValue);


}