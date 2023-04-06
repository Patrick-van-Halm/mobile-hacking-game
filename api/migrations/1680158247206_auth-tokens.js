/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('auth_tokens', {
        id: 'id',
        player_id: {
            type: 'integer',
            notNull: true,
            references: 'players',
            onDelete: 'cascade',
            unique: true,
        },
        token: { type: 'char(128)', notNull: true, unique: true },
        expires_at: { type: 'timestamp', notNull: true },
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
};

exports.down = pgm => {
    pgm.dropTable('auth_tokens');
};
