-- update existing sortie totals for the day
UPDATE rpt_sorties_over_time rpt 
	SET rpt.sorties = rpt.sorties + 
			(
				SELECT COUNT(id) 
				FROM tmp_gameevents 
				WHERE ucid = rpt.ucid AND role = rpt.airframe AND date = rpt.date AND event = "TAKEOFF"
			),
		rpt.kills = rpt.kills + 
			(
				SELECT COUNT(id) 
				FROM tmp_gameevents 
				WHERE ucid = rpt.ucid AND role = rpt.airframe AND date = rpt.date AND event = "KILL"
			),
		rpt.deaths = rpt.deaths + 
			(
				SELECT COUNT(DISTINCT sortie_id) 
				FROM tmp_gameevents 
				WHERE ucid = rpt.ucid AND role = rpt.airframe AND date = rpt.date AND (event = "DEAD" Or event = "CRASH" Or event = "PILOT_DEAD")
			),
		rpt.slingloads = rpt.slingloads + 
			(
				SELECT COUNT(DISTINCT sortie_id) 
				FROM tmp_gameevents 
				WHERE ucid = rpt.ucid AND role = rpt.airframe AND date = rpt.date AND event = "SLING_UNHOOK"
			),
		rpt.transport = rpt.transport + 
			(
				SELECT COUNT(DISTINCT sortie_id) 
				FROM tmp_gameevents 
				WHERE ucid = rpt.ucid AND role = rpt.airframe AND date = rpt.date AND event = "TRANSPORT_DISMOUNT"
			),
		rpt.resupplies = rpt.resupplies + 
			(
				SELECT COUNT(DISTINCT sortie_id) 
				FROM tmp_gameevents 
				WHERE ucid = rpt.ucid AND role = rpt.airframe AND date = rpt.date AND event = "DEPOT_RESUPPLY"
			);

			
-- insert new sortie totals for each new days/roles
INSERT INTO rpt_sorties_over_time (ucid, airframe, date, sorties, kills, deaths, slingloads, transport, resupplies)
SELECT 
	t.ucid, 
	t.role,
	t.date, 	
	SUM(CASE WHEN event = "TAKEOFF" THEN 1 ELSE 0 END) AS sorties,
	SUM(CASE WHEN event = "KILL" THEN 1 ELSE 0 END) AS kills,
	(
		SELECT COUNT(DISTINCT sortie_id) 
		FROM tmp_gameevents
		WHERE (event = "DEAD" Or event = "CRASH" Or event = "PILOT_DEAD") AND ucid = t.ucid AND role = t.role AND date = t.date
	) AS deaths,
	SUM(CASE WHEN event = "SLING_UNHOOK" THEN 1 ELSE 0 END) AS slingloads,
	SUM(CASE WHEN event = "TRANSPORT_DISMOUNT" THEN 1 ELSE 0 END) AS transport,
	SUM(CASE WHEN event = "DEPOT_RESUPPLY" THEN 1 ELSE 0 END) AS resupplies,
FROM tmp_gameevents t
WHERE t.ucid IS NOT NULL
GROUP BY t.ucid, t.date, t.role;



-- update existing 'TOTAL' records for each day 
UPDATE rpt_sorties_over_time rpt 
	INNER JOIN 
    (
		SELECT 
			ucid, date, 
			SUM(sorties) AS sorties, 
            SUM(kills) AS kills, 
            SUM(deaths) AS deaths, 
            SUM(slingloads) AS slingloads, 
            SUM(transport) AS transport,
            SUM(resupplies) AS resupplies
        FROM rpt_sorties_over_time
        WHERE airframe <> 'TOTAL'
        GROUP BY ucid, date
    ) AS t
    ON rpt.ucid = t.ucid AND rpt.date = t.date
	SET rpt.sorties = t.sorties,
		rpt.kills = t.kills,
		rpt.deaths = t.deaths,
		rpt.slingloads = t.slingloads,
		rpt.transport = t.transport,
		rpt.resupplies = t.resupplies
	WHERE rpt.airframe = 'TOTAL';
	
-- create new 'TOTAL' records for the day 
INSERT INTO rpt_sorties_over_time (ucid, airframe, date, sorties, kills, deaths, slingloads, transport, resupplies)
SELECT
	t.ucid,
	'TOTAL',
	t.date,
	(
		SELECT SUM(sorties) 
		FROM rpt_sorties_over_time rpt
		WHERE t.ucid = rpt.ucid AND t.date = rpt.date
	),
	(
		SELECT SUM(kills)
		FROM rpt_sorties_over_time rpt
		WHERE t.ucid = rpt.ucid AND t.date = rpt.date
	),
	(
		SELECT SUM(deaths)
		FROM rpt_sorties_over_time rpt
		WHERE t.ucid = rpt.ucid AND t.date = rpt.date
	),
	(
		SELECT SUM(slingloads)
		FROM rpt_sorties_over_time rpt
		WHERE t.ucid = rpt.ucid AND t.date = rpt.date
	),
	(
		SELECT SUM(transport)
		FROM rpt_sorties_over_time rpt
		WHERE t.ucid = rpt.ucid AND t.date = rpt.date
	),
	(
		SELECT SUM(resupplies)
		FROM rpt_sorties_over_time rpt
		WHERE t.ucid = rpt.ucid AND t.date = rpt.date
	)
FROM rpt_sorties_over_time t 
LEFT JOIN rpt_sorties_over_time tt
	ON tt.ucid = t.ucid AND tt.airframe = 'TOTAL' AND tt.date = t.date 
WHERE tt.airframe IS NULL
GROUP BY t.ucid, t.date