/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createFunction('updated_at_func', [], {
        returns: 'trigger',
        language: 'plpgsql',
        replace: true,
    }, `
        BEGIN
            NEW.updated_at = now();
            RETURN NEW;
        END;
    `);
};

exports.down = pgm => {
    pgm.dropFunction('updated_at_func', [], {
        ifExists: true,
        cascade: true,
    });
};
