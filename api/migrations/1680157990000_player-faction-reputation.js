/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('player_faction_reputation', {
        player_id: {
            type: 'integer',
            references: 'players',
            onDelete: 'cascade',
            primaryKey: true,
        },
        faction: {
            type: 'factions',
            primaryKey: true,
        },
        reputation: { type: 'integer', notNull: true },
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

    pgm.createTrigger('player_faction_reputation', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    }); 
};

exports.down = pgm => {
    pgm.dropTable('player_faction_reputation');
};
