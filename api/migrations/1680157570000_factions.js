/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createType('factions', ['Police', 'Hacker', 'Civilian']);
};

exports.down = pgm => {
    pgm.dropType('factions');
};
