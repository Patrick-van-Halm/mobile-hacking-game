/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('devices', {
        id: 'id',
        owner_person_id: {
            type: 'integer',
            notNull: true,
            references: 'persons',
            ondelete: 'cascade',
        },
        public_id: { type: 'varchar(255)', notNull: true, unique: true },
        name: { type: 'varchar(100)', notNull: true },
        ip: { type: 'varchar(15)', notNull: true },
        data: { type: 'jsonb', notNull: true },
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

    pgm.createTrigger('devices', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    });
};

exports.down = pgm => {
    pgm.dropTable('devices');
};
