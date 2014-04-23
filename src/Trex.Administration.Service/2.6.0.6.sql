/* NB: only run this on d60 customer */
UPDATE Tasks
SET TimeRegistrationTypeId = 1 --Projection
WHERE [TaskID] IN 
(
9700, --Planlagt Ferie
9702 --Planlagt Orlov
)
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
	VALUES ('2.6.0.6', GETDATE(), 'CTH', 'Moved some tasks to projections"')
GO 
