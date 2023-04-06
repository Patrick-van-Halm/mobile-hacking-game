/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('jobs', {
        id: 'id',
        contact_person_id: {
            type: 'integer',
            notNull: true,
            references: 'persons',
        },
        performing_player_id: {
            type: 'integer',
            references: 'players',
            ondelete: 'cascade',
        },
        public_id: { type: 'varchar(255)', notNull: true, unique: true },
        job_type: {
            type: 'job_type',
            notNull: true,
        },
        payout_amount: {
            type: 'integer',
            notNull: true,
        },
        min_level: {
            type: 'integer',
            notNull: true,
        },
        min_faction_reputation: {
            type: 'integer',
            notNull: true,
        },
        data: {
            type: 'jsonb',
            notNull: true,
        },
        deadline: {
            type: 'timestamp',
            notNull: true,
        },
        completed: { type: 'boolean', notNull: true, default: false },
        created_at: {
            type: 'timestamp',
            notNull: true,
            default: pgm.func('current_timestamp'),
        },
        updated_at: {
            type: 'timestamp',
            notNull: true,
            default: pgm.func('current_timestamp'),
        },
    });

    pgm.createTrigger('jobs', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    });
};

exports.down = pgm => {
    pgm.dropTable('jobs');
};
