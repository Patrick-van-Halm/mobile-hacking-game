/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('persons', {
        id: 'id',
        player_id: {
            type: 'integer',
            references: 'players',
            onDelete: 'cascade',
        },
        public_id: { type: 'varchar(255)', notNull: true, unique: true },
        faction: {
            type: 'factions',
        },
        name: { type: 'varchar(255)', notNull: true },
        email_address: {
            type: 'varchar(255)',
            notNull: true,
        },
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

    pgm.createTrigger('persons', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    });
};

exports.down = pgm => {
    pgm.dropTable('persons');
};
