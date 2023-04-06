import pgPromise from 'pg-promise';
const pgp = pgPromise({/* Initialization Options */});

export const db = pgp(process.env.DATABASE_URL || "");