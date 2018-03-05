SELECT id, g.server_id as event_server_id, g.session_id, s.server_id as session_server_id FROM raw_gameevents_log g
INNER JOIN session s
ON g.session_id = s.session_id
WHERE g.server_id <> s.server_id;

SELECT * FROM raw_gameevents_log l
LEFT JOIN session s
ON l.session_id = s.session_id AND l.server_id = s.server_id
WHERE s.server_id IS NULL;


-- Fix the server_id values as they do not match what is stored in the session table
UPDATE raw_gameevents_log AS l
INNER JOIN session s
ON l.session_id = s.session_id
SET l.server_id = s.server_id
WHERE l.server_id <> s.server_id;


SELECT * FROM session WHERE session_id = 17687