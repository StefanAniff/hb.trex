

CREATE view [dbo].[v_timereg_component] as
select  
dbo.RoundUpToNextQuarter(sum(a.BillableTime)) AS BillableTime,
datepart(Day,dbo.ConvertToSmallDate(A.ENDTIME)) as EndDay,
datepart(MONTH, dbo.ConvertToSmallDate(endtime)) as "Month",
datepart(year, dbo.ConvertToSmallDate(endtime)) as "Year",
	dbo.ConvertToSmallDate(A.ENDTIME) as EndDate,
a.Price,
	B.TASKNAME,
	C.PROJECTNAME,
	D.CUSTOMERNAME,
	G.COMPONENTNAME,
	u.username,
	a.billable
	

FROM dbo.TimeEntries A INNER JOIN dbo.Tasks B ON A.TASKID = B.TASKID INNER JOIN
	dbo.Projects c ON B.PROJECTID = C.PROJECTID INNER JOIN
	dbo.Users u on a.userid = u.userid inner join
	dbo.Customers D ON D.CUSTOMERID = C.CUSTOMERID left outer JOIN
	dbo.BugtrackerIntegrationTask E ON E.TrexTaskID = B.TASKID left outer JOIN
	GeminiV3.dbo.gemini_issuecomponents F ON E.GeminiTaskID = F.issueid left outer JOIN
	GeminiV3.dbo.gemini_components G ON G.COMPONENTID = F.COMPONENTID 

--where a.billable = 1 
group by a.Price,
	B.TASKNAME,
	C.PROJECTNAME,
	D.CUSTOMERNAME,
	G.COMPONENTNAME,
	u.username, 
	dbo.ConvertToSmallDate(A.ENDTIME),
	a.billable