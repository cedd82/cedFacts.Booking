
--CREATE DATABASE facts
--    WITH 
--    OWNER = postgres
--    ENCODING = 'UTF8'
--    LC_COLLATE = 'en_US.UTF-8'
--    LC_CTYPE = 'en_US.UTF-8'
--    TABLESPACE = pg_default
--    CONNECTION LIMIT = -1;

--CREATE TABLE public.process_event_data (
--	id serial NOT NULL,
--	user_code varchar(50) NOT NULL,
--	app_name varchar(50) NOT NULL,
--	process_url varchar(150) NOT NULL,
--	process_data jsonb NOT NULL,
--	create_date timestamp NOT NULL,
--	process_data_output jsonb NULL,
--	event_name varchar(50) NOT NULL,
--	status_code varchar(50) NOT NULL,
--	CONSTRAINT process_event_data_pk PRIMARY KEY (id)
--);

--CREATE TABLE public.sys_log (
--	id serial NOT NULL,
--	log_level varchar(1000) NULL,
--	log_logger varchar(255) NULL,
--	log_message text NULL,
--	log_exception text NULL,
--	log_source varchar(200) NULL,
--	app_type varchar(500) NULL,
--	device_name varchar(25) NULL,
--	log_date timestamp NOT NULL DEFAULT now(),
--	CONSTRAINT sys_log_pk PRIMARY KEY (id)
--);
