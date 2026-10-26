-- =============================================================
-- Database : miniproject
-- Purpose  : Initial schema (PostgreSQL)
-- Run in order from top to bottom.
-- Column names follow EF Core default (property name), quoted.
-- =============================================================

-- -------------------------------------------------------------
-- m_sample
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS m_sample (
    "Uid"           uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "DtCreated"     timestamp with time zone NOT NULL DEFAULT now(),
    "TxtCreatedBy"  text                     NULL,
    "DtUpdated"     timestamp with time zone NULL,
    "TxtUpdatedBy"  text                     NULL,
    "BitActive"     boolean                  NOT NULL DEFAULT true,
    "TxtCode"       varchar(50)              NOT NULL,
    "TxtName"       varchar(200)             NOT NULL,
    CONSTRAINT "PK_m_sample" PRIMARY KEY ("Uid")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_m_sample_TxtCode" ON m_sample ("TxtCode");

-- -------------------------------------------------------------
-- m_user
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS m_user (
    "Uid"              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "DtCreated"        timestamp with time zone NOT NULL DEFAULT now(),
    "TxtCreatedBy"     text                     NULL,
    "DtUpdated"        timestamp with time zone NULL,
    "TxtUpdatedBy"     text                     NULL,
    "BitActive"        boolean                  NOT NULL DEFAULT true,
    "TxtUsername"      varchar(50)              NOT NULL,
    "TxtPasswordHash"  varchar(500)             NOT NULL,
    "TxtFullName"      varchar(200)             NOT NULL,
    CONSTRAINT "PK_m_user" PRIMARY KEY ("Uid")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_m_user_TxtUsername" ON m_user ("TxtUsername");

-- -------------------------------------------------------------
-- m_region
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS m_region (
    "Uid"           uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "DtCreated"     timestamp with time zone NOT NULL DEFAULT now(),
    "TxtCreatedBy"  text                     NULL,
    "DtUpdated"     timestamp with time zone NULL,
    "TxtUpdatedBy"  text                     NULL,
    "BitActive"     boolean                  NOT NULL DEFAULT true,
    "TxtCode"       varchar(50)              NOT NULL,
    "TxtName"       varchar(200)             NOT NULL,
    CONSTRAINT "PK_m_region" PRIMARY KEY ("Uid")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_m_region_TxtCode" ON m_region ("TxtCode");

-- -------------------------------------------------------------
-- m_branch
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS m_branch (
    "Uid"           uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "DtCreated"     timestamp with time zone NOT NULL DEFAULT now(),
    "TxtCreatedBy"  text                     NULL,
    "DtUpdated"     timestamp with time zone NULL,
    "TxtUpdatedBy"  text                     NULL,
    "BitActive"     boolean                  NOT NULL DEFAULT true,
    "RefRegionId"   uuid                     NOT NULL,
    "TxtCode"       varchar(50)              NOT NULL,
    "TxtName"       varchar(200)             NOT NULL,
    "TxtAddress"    varchar(500)             NULL,
    CONSTRAINT "PK_m_branch" PRIMARY KEY ("Uid"),
    CONSTRAINT "FK_m_branch_m_region_RefRegionId" FOREIGN KEY ("RefRegionId")
        REFERENCES m_region ("Uid") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_m_branch_TxtCode" ON m_branch ("TxtCode");
CREATE INDEX IF NOT EXISTS "IX_m_branch_RefRegionId" ON m_branch ("RefRegionId");

-- -------------------------------------------------------------
-- t_budget
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS t_budget (
    "Uid"            uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "DtCreated"      timestamp with time zone NOT NULL DEFAULT now(),
    "TxtCreatedBy"   text                     NULL,
    "DtUpdated"      timestamp with time zone NULL,
    "TxtUpdatedBy"   text                     NULL,
    "BitActive"      boolean                  NOT NULL DEFAULT true,
    "RefBranchId"    uuid                     NOT NULL,
    "IntYear"        integer                  NOT NULL,
    "DecAmount"      numeric(18,2)            NOT NULL,
    "TxtDescription" varchar(500)             NULL,
    CONSTRAINT "PK_t_budget" PRIMARY KEY ("Uid"),
    CONSTRAINT "FK_t_budget_m_branch_RefBranchId" FOREIGN KEY ("RefBranchId")
        REFERENCES m_branch ("Uid") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_t_budget_RefBranchId" ON t_budget ("RefBranchId");

-- -------------------------------------------------------------
-- Sample data (regions)
-- -------------------------------------------------------------
INSERT INTO m_region ("TxtCode", "TxtName")
VALUES ('JKT', 'Jakarta'),
       ('SBY', 'Surabaya')
ON CONFLICT ("TxtCode") DO NOTHING;

-- -------------------------------------------------------------
-- Default admin user (admin / Admin@123) is NOT inserted here.
-- The application seeds it on startup via IAuthService.EnsureDefaultUserAsync().
-- -------------------------------------------------------------
