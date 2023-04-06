/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('players', {
        id: 'id',
        public_id: { type: 'varchar(255)', notNull: true, unique: true },
        email: { type: 'varchar(255)', notNull: true },
        password: { type: 'varchar(255)', notNull: true },
        level: { type: 'integer', notNull: true, default: 0 },
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

    pgm.createTrigger('players', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    });
};

exports.down = pgm => {
    pgm.dropTable('players');
};
