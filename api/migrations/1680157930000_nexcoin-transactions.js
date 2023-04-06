/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createTable('nexcoin_transactions', {
        id: 'id',
        sender_person_id: {
            type: 'integer',
            references: 'persons',
            ondelete: 'set null',
        },
        receiver_person_id: {
            type: 'integer',
            references: 'persons',
            ondelete: 'set null',
        },
        job_id: {
            type: 'integer',
            references: 'jobs',
            ondelete: 'cascade',
        },
        public_id: { type: 'varchar(255)', notNull: true, unique: true },
        amount: { type: 'integer', notNull: true },
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

    pgm.createTrigger('nexcoin_transactions', 'updated_at_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE'],
        level: 'ROW',
        function: 'updated_at_func',
    }); 

    pgm.createFunction('delete_when_receiver_and_sender_are_null', [], {
        returns: 'trigger',
        language: 'plpgsql',
        replace: true,
    }, 
    `
        BEGIN
            IF NEW.receiver_person_id IS NULL AND NEW.sender_person_id IS NULL THEN
                DELETE FROM nexcoin_transactions WHERE id = NEW.id;
            END IF;
            RETURN NEW;
        END;
    `);

    pgm.createTrigger('nexcoin_transactions', 'delete_when_receiver_and_sender_are_null_trigger', {
        when: 'BEFORE',
        operation: ['UPDATE', 'DELETE', 'INSERT'],
        level: 'ROW',
        function: 'delete_when_receiver_and_sender_are_null',
    });
};

exports.down = pgm => {
    pgm.dropTable('nexcoin_transactions');
    pgm.dropFunction('delete_when_receiver_and_sender_are_null', [], {
        ifExists: true,
        cascade: true,
    });
};
