/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('emails', {
        id: 'id',
        sender_person_id: {
            type: 'integer',
            notNull: true,
            references: 'persons',
        },
        receiver_person_id: {
            type: 'integer',
            notNull: true,
            references: 'persons',
        },
        prev_email_id: {
            type: 'integer',
            references: 'emails',
            ondelete: 'cascade',
        },
        job_id: {
            type: 'integer',
            references: 'jobs',
            ondelete: 'cascade'
        },
        public_id: { type: 'varchar(255)', notNull: true, unique: true },
        subject: { type: 'varchar(255)', notNull: true },
        body: { type: 'text', notNull: true },
        has_read: { type: 'boolean', notNull: true, default: false },
        deleted: { type: 'boolean', notNull: true, default: false },
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

    pgm.createTrigger('emails', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    });
};

exports.down = pgm => {
    pgm.dropTable('emails');
};
