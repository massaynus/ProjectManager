use ProjectManagerDB
--delete from ActionLog where RequestDate between '2020-05-16 00:08:59.573' and '2020-05-16 03:48:20.350' and ActionMethod = 'GET'
--select ActionName, called = COUNT(ActionName) from ActionLog group by ActionName


select DATEPART(HOUR, RequestDate) as Hour, DATEPART(MINUTE, RequestDate) as Minute, DATEPART(SECOND, RequestDate) as Second, COUNT(*) as [Req / Sec]
from ActionLog 
group by DATEPART(HOUR, RequestDate), DATEPART(MINUTE, RequestDate), DATEPART(SECOND, RequestDate)
order by [Req / Sec] desc
