CREATE OR ALTER PROCEDURE pGetThreeClosestBranches (@lat1 float, @lng1 float)
AS
SELECT TOP 3 Branches.Id, BRanches.Street, Branches.City, Branches.Region, Branches.Longitude, Branches.Latitude, SQRT(POWER(Branches.Latitude - @lat1, 2) + POWER(Branches.Longitude - @lng1, 2)) * 62.1371192 AS Distance FROM Branches ORDER BY Distance
exec pGetThreeClosestBranches 43.653226, -79.3832